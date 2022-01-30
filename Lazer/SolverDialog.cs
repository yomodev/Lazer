using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Combinatorics.Collections;

namespace Lazer
{
    public partial class SolverDialog : Form
    {
        internal ControlBoard controlboard;
        private object[] values;
        Stopwatch sw1 = new Stopwatch();
        Stopwatch sw2 = new Stopwatch();
        long prev = 0;
        


        public SolverDialog()
        {
            InitializeComponent();
            
        }

        

        // setup solver
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.ProgressChanged += bw_ProgressChanged;
            sw1.Start();
            sw2.Start();
            bw.RunWorkerAsync(controlboard.board);

            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            timer.Start();
        }



        // each second update screen and progress
        void timer_Tick(object sender, EventArgs e)
        {
            if (bw.IsBusy && values != null)
            { 
                long i = (long)values[0];
                long tot = (long)values[1];
                
                progressBar1.Value = unchecked((int)(i*100/tot));
                this.Text = "Solving " + progressBar1.Value + "%";

                label1.Text = sw1.Elapsed + "\n" + tot + "\n" + i;
                label1.Text += "\n" + (i - prev)/sw2.ElapsedMilliseconds * 1000 + " / " + (timer.Interval/1000) + " sec";
                sw2.Restart();
                prev = i;

                // update board using last permutation
                if (cb_redraw.Checked)
                {
                    Board b = values[2] as Board;
                    controlboard.board = b.copy();
                    
                    // set pinned
                    for (int x = 0; x < b.width; x++)
                    {
                        for (int y = 0; y < b.height; y++)
                        {
                            controlboard.board[x,y].isPinned = b[x, y].isPinned;
                        }
                    }

                    controlboard.Test();
                }
                
            }
        }



        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            values = e.UserState as object[];
        }
        


        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine(e.ToString());

            if (e.Cancelled == true)
            {
                //resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error");
            }
            else
            {
                long solutions = (long)e.Result;
                if(solutions == 0)
                {
                    MessageBox.Show("No solution found.");
                }
                else
                {
                    MessageBox.Show("Found " + solutions + " solution(s).\nCheck screenshots in program\'s folder.", "Yesss!!!");
                }
            }

            // TODO
            // display solved board
            Close();
        }



        internal void solve(ControlBoard cb, LazerForm owner)
        {
            controlboard = cb;
            ShowDialog(owner);
        }



        // do the hard work in another thread
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Board board = e.Argument as Board;
            List<Block> movable = new List<Block>();
            
            int w = board.width;
            int h = board.height;
            Board source = Board.create(w, h);
            Block b;
            
            // create list of movable objects
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    b = board[x, y];
                    // store movable items
                    if (b.isMovable && !b.isPinned)
                    {
                        movable.Add(b);
                        Debug.WriteLine("adding " + b.ToString());
                    }
                    else
                    {
                        // put in the new board pinned and unmovable elements
                        source[x, y] = b;
                    }
                }
            }

            // calculate permutatations
            Permutations<Block> perms = new Permutations<Block>(movable, GenerateOption.WithoutRepetition);
            string format1 = "Permutations of {0} movable objects in board: {1}";
            long i = 1;
            long solutions = 0;
            long tot = perms.Count;
            Console.WriteLine(String.Format(format1, movable.Count, tot));
            // create a board copy
            Board testBoard = source.copy();

            // foreach permutation
            foreach (List<Block> p in perms)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                // fill board
                int j = 0;
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        b = source[x, y];
                        testBoard.setBlock(x, y, b is Empty && !b.isPinned ? p[j++] : source[x, y]);
                    }
                }

                // test board
                if (testBoard.analyze() && testBoard.process(false, i + "."))
                {
                    // save solution!
                    string s = i + " ";
                    solutions++;
                    p.ForEach(m => s += m.ToString() + ", ");

                    //MessageBox.Show(s);Guid.NewGuid()
                    testBoard.saveScreenshot(solutions + "-" + i + "-" + sw1.ElapsedMilliseconds + ".png");
                    Console.WriteLine("** " + s);
                }

                i++;
                //if(i%1000==0)
                bw.ReportProgress(0, new Object[] { i, tot, testBoard });
            }

            e.Result = solutions;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (bw.IsBusy)
            {
                bw.CancelAsync();
            }

            if (sw1.IsRunning)
            {
                sw1.Stop();
            }

            if (sw2.IsRunning)
            {
                sw2.Stop();
            }
        }


    }
}
