using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dilibaier0504ERSWindowsFormApp
{
    public partial class EmployeeRecordsForm : Form
    {
        private TreeNode tvRootNode;

        public EmployeeRecordsForm()
        {
            InitializeComponent();
            PopulateTreeView();
            InitalizeListControl();
        }

        private void EmployeeRecordsForm_Load(object sender, EventArgs e)
        {

        }
        private void PopulateTreeView()
        {
            statusBarPanel1.Tag = "Refreshing Employee Code. Please wait...";
            this.Cursor = Cursors.WaitCursor;
            treeView1.Nodes.Clear();
            tvRootNode = new TreeNode("Employee Records");
            this.Cursor = Cursors.Default;
            treeView1.Nodes.Add(tvRootNode);

            TreeNodeCollection nodeCollection=tvRootNode.Nodes;
            XmlTextReader reader=new XmlTextReader("E:\\外交\\大三下\\MyRepos\\Dilibaier0504ERSWindowsFormApp\\Dilibaier0504ERSWindowsFormApp\\EmpRec.xml");
       
            reader.MoveToElement();
            try
            {
                while (reader.Read())
                {
                    if(reader.HasAttributes && reader.NodeType == XmlNodeType.Element)
                    {
                      reader.MoveToElement();//<EmpRecordsData>
                        reader.MoveToElement();//Ecode
                        reader.MoveToAttribute("Id"); //Id = "E001
                        String strVal = reader.Value;//E001
                        reader.Read();
                        reader.Read();
                        if(reader.Name == "Dept")
                        {
                            reader.Read();
                        }
                        //creat a child node
                        TreeNode EcodeNode = new TreeNode(strVal);
                        //add the node
                        nodeCollection.Add(EcodeNode);
                    }
                }
                statusBarPanel1.Text = "Click on an employee code to see their record.";
            }
            catch(XmlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void InitalizeListControl()
        {
            listView1.Clear();
            listView1.Columns.Add("Employee Name",255,HorizontalAlignment.Left);
            listView1.Columns.Add("Date of Join",70,HorizontalAlignment.Right);
            listView1.Columns.Add("Gread", 105, HorizontalAlignment.Left);
            listView1.Columns.Add("Salary", 105, HorizontalAlignment.Left);
        }
        private void PopulateListView(TreeNode crrNode)
        {
            InitalizeListControl();
            XmlTextReader listRead =new XmlTextReader("E:\\外交\\大三下\\MyRepos\\Dilibaier0504ERSWindowsFormApp\\Dilibaier0504ERSWindowsFormApp\\EmpRec.xml");
            listRead.MoveToElement();
            while (listRead.Read())
            {
                String strNodeName;
                String strNodePath;
                String name;
                String gread;
                String doj;
                String sal;
                String[] strItemsArr=new String[4];
                listRead.MoveToFirstAttribute();//Id=E001
                strNodeName=listRead.Value;
                strNodePath = crrNode.FullPath.Remove(0, 17);
                if(strNodeName == strNodePath)
                {
                    ListViewItem lvi;
                    listRead.MoveToNextAttribute();
                    name=listRead.Value;//Name="Michael Perry"
                    lvi =listView1.Items.Add(listRead.Value);
                    listRead.Read();
                    listRead.Read();
                    
                    listRead.MoveToFirstAttribute();
                    doj=listRead.Value;//DateofJoin = "02-02-1999
                    lvi.SubItems.Add(doj); 

                    listRead.MoveToNextAttribute();
                    gread = listRead.Value;//Gread="A"
                    lvi.SubItems.Add(gread);

                    listRead.MoveToNextAttribute();
                    sal = listRead.Value;//Salary="1750"
                    lvi.SubItems.Add(sal);

                    listRead.MoveToNextAttribute();
                    listRead.MoveToElement();
                    listRead.ReadString();
                }
            }
        }//end

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currNode=e.Node;
            if(tvRootNode==currNode)
            {
                InitalizeListControl();
                statusBarPanel1.Text = "Double Click the Employee Records";
                return;
            }
            else
            {
                statusBarPanel1.Text = "Click an Employee code to view individual record";

            }
            PopulateListView(currNode);
        }
    }
}
