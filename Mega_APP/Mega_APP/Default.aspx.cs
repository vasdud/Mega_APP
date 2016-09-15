using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Mega_APP
{
    public partial class _Default : System.Web.UI.Page
    {
        const string myInitCatalog = "; Initial Catalog=";
        const string myQuery = "SELECT TOP 10 * from ";

        struct DBItems
        { 
            public string Name;
            public int ID;
            public DateTime CreatedDate;
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                using (var aCon = new SqlConnection(ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString()))
                {
                    List<DBItems> aListDBs = new List<DBItems>();
                    aCon.Open();
                    DataTable aDBs = aCon.GetSchema("Databases");
                    TreeNode aChild = new TreeNode
                    {
                        Text = "Databases",
                        Value = "0"
                    };
                    TreeV.Nodes.Add(aChild);
                    foreach (DataRow aDBr in aDBs.Rows)
                    {
                        string aConnStr = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString() + myInitCatalog + aDBr.ItemArray[0].ToString();
                        PopulateTreeView(aDBr, aConnStr, aChild);
                        /*DBItems aDBI = new DBItems();
                        aDBI.Name = aDBr.ItemArray[0].ToString();
                        aDBI.ID = Int32.Parse(aDBr.ItemArray[1].ToString());
                        aDBI.CreatedDate = DateTime.Parse(aDBr.ItemArray[1].ToString());
                        aListDBs.Add(aDBI);*/
                    }
                }
                //////////
                //DataTable dt = this.GetData("SELECT [Personal Number], [Name] FROM [Users]");
                //this.PopulateTreeView(dt, 0, null);
            }
        }

        private void PopulateTreeView(DataRow theDBr, string theConnStr, TreeNode theChild)
        {
            int aCount = 0;
            TreeNode aChild = new TreeNode
            {
                Text = theDBr.ItemArray[0].ToString(),
                Value = theDBr.ItemArray[1].ToString()
            };
            theChild.ChildNodes.Add(aChild);

            using (var aCon = new SqlConnection(theConnStr))
            {
                TreeNode aCh;
                aCon.Open();
                DataTable aDBs = aCon.GetSchema("Tables");
                foreach (DataRow aDBr in aDBs.Rows)
                {
                    if (aDBr.ItemArray[3].ToString().Equals("BASE TABLE"))
                    {
                        aCount++;
                        aCh = new TreeNode
                        {
                            Text = aDBr.ItemArray[2].ToString(),
                            Value = aCount.ToString()
                        };
                        aChild.ChildNodes.Add(aCh);
                    }
                }
                /*if (aCount == 0)
                {
                    aCh = new TreeNode
                    {
                        Text = "",
                        Value = ""
                    };
                    aChild.ChildNodes.Add(aCh);
                    aChild.Expanded = false;
                    aChild.ShowCheckBox = false;
                }*/
            }
            //TreeV.Nodes.Add(aChild);
            /*if (parentId == 0)
            {
                TreeView1.Nodes.Add(child);
                DataTable dtChild = this.GetData("SELECT [Personal Number], [Name] FROM [Users] WHERE [Personal Number] = " + child.Value);
                PopulateTreeView(dtChild, int.Parse(child.Value), child);
            }
            else
            {
                treeNode.ChildNodes.Add(child);
            }*/
        }

        protected void TreeV_SelectedNodeChanged(object sender, EventArgs e)
        {
            string aQuery = string.Empty;
            string ICatalog = string.Empty;
            TreeNode aSNode = ((TreeView)sender).SelectedNode;
            if (aSNode.Parent != null)
            {
                TreeNode aPNode = aSNode.Parent;
                ICatalog = aPNode.Text;
            }
            aQuery = myQuery + aSNode.Text;
            DataTable dt = new DataTable();
            string dbConn = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString() + myInitCatalog + ICatalog;
            using (SqlConnection con = new SqlConnection(dbConn))
            {
                using (SqlCommand cmd = new SqlCommand(aQuery))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }
            GView.DataSource = dt;
            GView.DataBind();
        }

        /*private DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            string dbConn = ConfigurationManager.ConnectionStrings["cs_toSQL"].ToString();
            using (SqlConnection con = new SqlConnection(dbConn))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }*/
    }
}
