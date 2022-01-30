using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
//using LuaSpace;

namespace Lazer
{
    public partial class LazerForm : Form
    {
        //LuaSharp l;
        string customBoards = "customBoards.xml";
        string gameBoards = "gameBoards.xml";
        private Point prevSelectedPoint = Point.Empty;
        Dictionary<string,XmlElement> dictBoard = new Dictionary<string,XmlElement>();
            

        public LazerForm()
        {
            InitializeComponent();

            controlBoard.OnCheck += controlBoard_OnCheck;
            controlBoard.OnSelectionChange += controlBoard_OnSelectionChange;

            // init listview
            FillListBlockView();

            // load levels
            loadLevels();

            // load first/last

            //ListViewItem lvi = new ListViewItem("test1");
            //lvi.ImageList = new ImageList(
            //lv.Items.Add(lvi);
            
            /*
            l = new LuaSharp();
            l.push(board, "board");
            l.push(this, "lazerform");
            string f = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\console.lua";

            Debug.WriteLine("dofile " + f );
            Status s = l.dofile(f);
            Debug.WriteLine(s.ToString());
            */


            //controlBoard.Test();
            if (listBoxBoards.Items.Count > 0)
            {
                listBoxBoards.SelectedIndex = 0;
                controlBoard.LoadBoard(dictBoard[listBoxBoards.Text]);
                textBoxLevel.Text = controlBoard.board.name;
            }
        }



        void controlBoard_OnSelectionChange(object sender, Point selectedPoint)
        {
            if (controlBoard.isEditing)
            {
                Block b = null;
                if (radio_place.Checked)
                {
                    string typeName = listBlockView.SelectedItems[0].Name;
                    Type type = Type.GetType(typeName);
                    type = type == null ? Type.GetType("Lazer." + char.ToUpper(typeName[0]) + typeName.Substring(1)) : type;
                    if (type == null)
                    {
                        throw new Exception("invalid type " + typeName);
                    }

                    b = (Block)Activator.CreateInstance(type);
                    
                    if (b != null)
                    {
                        if (b.GetType() == controlBoard[selectedPoint].GetType() && b is IRotable)
                        {
                            (controlBoard[selectedPoint] as IRotable).rotate();
                        }
                        else
                        {
                            controlBoard.setBlock(selectedPoint, b);
                        }
                    }
                }
                else if (prevSelectedPoint == selectedPoint && controlBoard[selectedPoint]is IRotable)
                {
                    (controlBoard[selectedPoint] as IRotable).rotate();
                }
                
                //controlBoard.Redraw(false);
                controlBoard.Test();
                prevSelectedPoint = selectedPoint;
                propertyGrid1.SelectedObject = controlBoard.selectedBlock;
            }

            
        }



        private void FillListBlockView()
        {
            /*listBlockView.Items.Add("none", "Select - Move", "none");
            imageList1.Images.Add("none", Texture.extract(TextureType.Pointer, 0));
            */
            imageList1.Images.Add("floor", Texture.extract(TextureType.Floor, 0));
            listBlockView.Items.Add("Lazer.Empty", "Floor", "floor");

            imageList1.Images.Add("wall", Texture.extract(TextureType.Wall, 0));
            listBlockView.Items.Add("Lazer.Wall", "Wall", "wall");

            imageList1.Images.Add("splitter", Texture.extract(TextureType.Splitter, 0));
            listBlockView.Items.Add("Lazer.Splitter", "Splitter", "splitter");

            imageList1.Images.Add("emitter", Texture.extract(TextureType.Emitter, 0));
            listBlockView.Items.Add("Lazer.Emitter", "Emitter", "emitter");

            imageList1.Images.Add("receiver", Texture.extract(TextureType.ReceiverOff, 0));
            listBlockView.Items.Add("Lazer.Receiver", "Receiver", "receiver");

            imageList1.Images.Add("colorblock", Texture.extract(TextureType.ColorBlock, 0));
            listBlockView.Items.Add("Lazer.ColorBlock", "ColorBlock", "colorblock");

            imageList1.Images.Add("mirror", Texture.extract(TextureType.Mirror, 0));
            listBlockView.Items.Add("Lazer.Mirror", "Mirror", "mirror");

            imageList1.Images.Add("prism", Texture.extract(TextureType.Prism, 90));
            listBlockView.Items.Add("Lazer.Prism", "Prism", "prism");

            imageList1.Images.Add("tetrablock", Texture.extract(TextureType.Tetra, 90));
            listBlockView.Items.Add("Lazer.TetraBlock", "TetraBlock", "tetrablock");

            imageList1.Images.Add("filterhv", Texture.extract(TextureType.FilterHV, 0));
            listBlockView.Items.Add("Lazer.FilterHV", "FilterHV", "filterhv");

            imageList1.Images.Add("filter90", Texture.extract(TextureType.Filter90, 0));
            listBlockView.Items.Add("Lazer.Filter90", "Filter90", "filter90");

            imageList1.Images.Add("adder", Texture.extract(TextureType.Plus, 0));
            listBlockView.Items.Add("Lazer.Adder", "Adder", "adder");

            imageList1.Images.Add("sub", Texture.extract(TextureType.Minus, 0));
            listBlockView.Items.Add("Lazer.TetraBlock", "Subtractor", "sub");

            imageList1.Images.Add("multi", Texture.extract(TextureType.Multiply, 0));
            listBlockView.Items.Add("Lazer.Multiplier", "Multiplier", "multi");

            listBlockView.Items[0].Selected = true;

        }



        void controlBoard_OnCheck(object sender, bool win)
        {
            if (win && !controlBoard.isEditing)
            { 
                if(listBoxBoards.SelectedIndex+1 < listBoxBoards.Items.Count)
                {
                    if(MessageBox.Show("You Win!!!\nProceed with next level?","Congratulations", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //listBoxBoards.
                        //MessageBox.Show("puoi selezionarti il prossimo livello senza il mio aiuto...");
                        listBoxBoards.SelectedIndex++;
                        controlBoard.LoadBoard(dictBoard[listBoxBoards.Text]);
                        textBoxLevel.Text = controlBoard.board.name;
                    }
                }
            }
            
            this.Text = win ? "You win" : "Lazer - by Yomo";
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            controlBoard.isEditing = tabControl1.SelectedIndex == 1;
            //Debug.WriteLine("editing = " + editing);
        }


        private void btnSolve_Click(object sender, EventArgs e)
        {
            SolverDialog pd = new SolverDialog();
            pd.solve(controlBoard, this);
            //controlBoard.solve();
        }

        
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //controlBoard.Redraw(false);
            controlBoard.Test();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBoxLevel.Text.Length == 0)
            {
                MessageBox.Show("Invalid level name");
                return;
            }

            Board b = controlBoard.board;
            b.name = textBoxLevel.Text;

            XmlDocument xdoc = new XmlDocument();
            XmlNode levels;
            if (File.Exists(customBoards))
            {
                xdoc.Load(customBoards);
                levels = xdoc.FirstChild;
            }
            else 
            {
                levels = xdoc.CreateElement("levels");
                xdoc.AppendChild(levels);
                
            }
            
            // if level with same name already exists...
            XmlNodeList nlist = xdoc.SelectNodes("/levels/board[@name='" + b.name + "']");
            if (nlist.Count > 0)
            {
                if (DialogResult.Cancel == MessageBox.Show("A level with same name already exists.\nOverwrite?", "Question", MessageBoxButtons.OKCancel))
                {
                    return;
                }

                // replace
                levels.ReplaceChild(b.serialize(xdoc), nlist[0]);
            }
            else
            { 
                levels.AppendChild(b.serialize(xdoc));
            }

            xdoc.Save(customBoards);

            loadLevels();
        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (textBoxLevel.Text.Length == 0)
            {
                MessageBox.Show("Invalid level name");
                return;
            }

            if (MessageBox.Show("Remove level?\n" + textBoxLevel.Text, "Question", 
                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            MessageBox.Show("TODO!");
        }


        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //reloadBoards();
        }


        private void loadLevels()
        {
            dictBoard.Clear();
            XmlDocument xdoc = new XmlDocument();
            /*
            if (File.Exists(gameBoards))
            {
                xdoc.Load(gameBoards);
            }
            else
            {
                Assembly a = Assembly.GetExecutingAssembly();
                Stream s = a.GetManifestResourceStream("Lazer.Resource." + gameBoards);
                xdoc.Load(s);
            }

            fillListBoxLevels(xdoc);*/

            if (File.Exists(customBoards))
            {
                //customBoards
                xdoc.Load(customBoards); 
                fillListBoxLevels(xdoc);
            }

            // update listbox levels
            updateListBoxBoards();
        }


        private void updateListBoxBoards()
        {
            listBoxBoards.Items.Clear();
            foreach (var item in dictBoard.Keys)
            {
                listBoxBoards.Items.Add(item);
            }
        }


        private void fillListBoxLevels(XmlDocument xdoc)
        {
            foreach ( XmlElement xboard in xdoc.GetElementsByTagName("board"))
            {
                string name = xboard.GetAttribute("name");
                if (!dictBoard.ContainsKey(name))
                { 
                    dictBoard.Add(name, xboard);
                }
                
            }
        }

        

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (listBoxBoards.SelectedIndex >= 0)
            {
                controlBoard.LoadBoard(dictBoard[listBoxBoards.Text]);
                textBoxLevel.Text = controlBoard.board.name;
            }
        }

        
        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            listBlockView.Enabled = radio_place.Checked;

            if (listBlockView.Enabled)
            {
                listBlockView.Select();
                if (listBlockView.SelectedIndices.Count == 0)
                {
                    listBlockView.Items[0].Selected = true;
                }
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            controlBoard.board.clear(true);
            controlBoard.Test();
            textBoxLevel.Clear();
        }



        private void listBoxBoards_Click(object sender, EventArgs e)
        {
            if (listBoxBoards.SelectedIndex >= 0)
            {
                controlBoard.LoadBoard(dictBoard[listBoxBoards.Text]);
                textBoxLevel.Text = controlBoard.board.name;
            }
        }


        
    }
}
