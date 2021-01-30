using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Input
{
    public partial class productionPlanning : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillWorkCenter();

                productionPlanningCalenderTextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
                productionPlanningPriorityCalenderTextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "productionPlanningWCNameDropDownList")
            {
                ((DropDownList)sender).Items.Remove("".Trim());

                productionPlanningWCIDLabel.Text = myWebService.getID("wcMaster", "name", ((DropDownList)sender).SelectedItem.Text);

                fillGridView(((DropDownList)sender).SelectedItem.ToString());
            }
            else if (((DropDownList)sender).ID == "productionPlanningPriorityWCNameDropDownList")
            {
                ((DropDownList)sender).Items.Remove("".Trim());

                productionPlanningPriorityWCIDLabel.Text = myWebService.getID("wcMaster", "name", ((DropDownList)sender).SelectedItem.Text);

                fillGridView(((DropDownList)sender).SelectedItem.ToString(), "A");
                setPriority();
            }

        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "productionPlanningSaveButton")
            {
                //Code to save the production planning

                for (int i = 0; i < productionPlanningGridView.Rows.Count; i++)
                {
                    GridViewRow gridViewRow = productionPlanningGridView.Rows[i];

                    string recipeID = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridRecipeIDLabel"))).Text.Trim();
                    string componentID = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridComponentIDLabel"))).Text.Trim();


                    string shiftAPlan = (((TextBox)gridViewRow.Cells[1].FindControl("productionPlanningGridShiftAPlanTextBox"))).Text.Trim();
                    save(productionPlanningWCIDLabel.Text, componentID, recipeID, shiftAPlan, "A", "");

                    string shiftBPlan = (((TextBox)gridViewRow.Cells[1].FindControl("productionPlanningGridShiftBPlanTextBox"))).Text.Trim();
                    save(productionPlanningWCIDLabel.Text, componentID, recipeID, shiftBPlan, "B", "");

                    string shiftCPlan = (((TextBox)gridViewRow.Cells[1].FindControl("productionPlanningGridShiftCPlanTextBox"))).Text.Trim();
                    save(productionPlanningWCIDLabel.Text, componentID, recipeID, shiftCPlan, "C", "");

                }

            }
            else if (((Button)sender).ID == "productionPlanningCancelButton")
            {
            }
        }

        protected void UpDownButton_Click(object sender, ImageClickEventArgs e)
        {
            int rowIndex = 0;
            int tempRowIndex = 0;

            string ID = "";
            string swapID = "";
            string tempID = "";


            if (((ImageButton)sender).ID == "productionPlanningPriorityShiftAUPImageButton")
            {
                if ((getRowPosition("A") == "first") || (productionPlanningPriorityShiftAGridView.Rows.Count == 0))
                {
                }
                else
                {
                    foreach (GridViewRow row in productionPlanningPriorityShiftAGridView.Rows)
                    {
                        if (((RadioButton)row.FindControl("productionPlanningPriorityShiftARadioButton")).Checked == true)
                        {
                            rowIndex = tempRowIndex;
                            swapID = tempID;

                            ID = ((Label)row.FindControl("productionPlanningPriorityShiftAIDLabel")).Text;
                        }
                        tempID = ((Label)row.FindControl("productionPlanningPriorityShiftAIDLabel")).Text;

                        tempRowIndex++;
                    }
                    Swap("UP", Convert.ToInt32(rowIndex.ToString()), ID, swapID);
                    fillGridView(productionPlanningPriorityWCNameDropDownList.SelectedItem.ToString(), "A");

                    ((RadioButton)productionPlanningPriorityShiftAGridView.Rows[rowIndex - 1].FindControl("productionPlanningPriorityShiftARadioButton")).Checked = true;
                    productionPlanningPriorityShiftAGridView.Rows[rowIndex - 1].BackColor = System.Drawing.Color.Yellow;
                }
            }
            else if (((ImageButton)sender).ID == "productionPlanningPriorityShiftADownImageButton")
            {
                bool flag = false;

                if ((getRowPosition("A") == "last") || (productionPlanningPriorityShiftAGridView.Rows.Count == 0))
                {
                }
                else
                {
                    foreach (GridViewRow row in productionPlanningPriorityShiftAGridView.Rows)
                    {
                        if (flag == true)
                        {
                            tempID = ((Label)row.FindControl("productionPlanningPriorityShiftAIDLabel")).Text;
                            swapID = tempID;
                            flag = false;
                        }

                        if (((RadioButton)row.FindControl("productionPlanningPriorityShiftARadioButton")).Checked == true)
                        {
                            rowIndex = tempRowIndex;
                            flag = true;

                            ID = ((Label)row.FindControl("productionPlanningPriorityShiftAIDLabel")).Text;
                        }
                        tempID = ((Label)row.FindControl("productionPlanningPriorityShiftAIDLabel")).Text;

                        tempRowIndex++;
                    }
                    Swap("DOWN", Convert.ToInt32(rowIndex.ToString()), ID, swapID);
                    fillGridView(productionPlanningPriorityWCNameDropDownList.SelectedItem.ToString(), "A");

                    ((RadioButton)productionPlanningPriorityShiftAGridView.Rows[rowIndex + 1].FindControl("productionPlanningPriorityShiftARadioButton")).Checked = true;
                    productionPlanningPriorityShiftAGridView.Rows[rowIndex + 1].BackColor = System.Drawing.Color.Yellow;
                }
            }

            // Block for Shift B //

            if (((ImageButton)sender).ID == "productionPlanningPriorityShiftBUPImageButton")
            {
                if ((getRowPosition("B") == "first") || (productionPlanningPriorityShiftBGridView.Rows.Count == 0))
                {
                }
                else
                {
                    foreach (GridViewRow row in productionPlanningPriorityShiftBGridView.Rows)
                    {
                        if (((RadioButton)row.FindControl("productionPlanningPriorityShiftBRadioButton")).Checked == true)
                        {
                            rowIndex = tempRowIndex;
                            swapID = tempID;

                            ID = ((Label)row.FindControl("productionPlanningPriorityShiftBIDLabel")).Text;
                        }
                        tempID = ((Label)row.FindControl("productionPlanningPriorityShiftBIDLabel")).Text;

                        tempRowIndex++;
                    }
                    Swap("UP", Convert.ToInt32(rowIndex.ToString()), ID, swapID);
                    fillGridView(productionPlanningPriorityWCNameDropDownList.SelectedItem.ToString(), "B");

                    ((RadioButton)productionPlanningPriorityShiftBGridView.Rows[rowIndex - 1].FindControl("productionPlanningPriorityShiftBRadioButton")).Checked = true;
                    productionPlanningPriorityShiftBGridView.Rows[rowIndex - 1].BackColor = System.Drawing.Color.Yellow;
                }
            }
            else if (((ImageButton)sender).ID == "productionPlanningPriorityShiftBDownImageButton")
            {
                bool flag = false;

                if ((getRowPosition("B") == "last") || (productionPlanningPriorityShiftBGridView.Rows.Count == 0))
                {
                }
                else
                {
                    foreach (GridViewRow row in productionPlanningPriorityShiftBGridView.Rows)
                    {
                        if (flag == true)
                        {
                            tempID = ((Label)row.FindControl("productionPlanningPriorityShiftBIDLabel")).Text;
                            swapID = tempID;
                            flag = false;
                        }

                        if (((RadioButton)row.FindControl("productionPlanningPriorityShiftBRadioButton")).Checked == true)
                        {
                            rowIndex = tempRowIndex;
                            flag = true;

                            ID = ((Label)row.FindControl("productionPlanningPriorityShiftBIDLabel")).Text;
                        }
                        tempID = ((Label)row.FindControl("productionPlanningPriorityShiftBIDLabel")).Text;

                        tempRowIndex++;
                    }
                    Swap("DOWN", Convert.ToInt32(rowIndex.ToString()), ID, swapID);
                    fillGridView(productionPlanningPriorityWCNameDropDownList.SelectedItem.ToString(), "B");

                    ((RadioButton)productionPlanningPriorityShiftBGridView.Rows[rowIndex + 1].FindControl("productionPlanningPriorityShiftBRadioButton")).Checked = true;
                    productionPlanningPriorityShiftBGridView.Rows[rowIndex + 1].BackColor = System.Drawing.Color.Yellow;
                }
            }

            // Block for shift C //

            if (((ImageButton)sender).ID == "productionPlanningPriorityShiftCUPImageButton")
            {
                if ((getRowPosition("C") == "first") || (productionPlanningPriorityShiftCGridView.Rows.Count == 0))
                {
                }
                else
                {
                    foreach (GridViewRow row in productionPlanningPriorityShiftCGridView.Rows)
                    {
                        if (((RadioButton)row.FindControl("productionPlanningPriorityShiftCRadioButton")).Checked == true)
                        {
                            rowIndex = tempRowIndex;
                            swapID = tempID;

                            ID = ((Label)row.FindControl("productionPlanningPriorityShiftCIDLabel")).Text;
                        }
                        tempID = ((Label)row.FindControl("productionPlanningPriorityShiftCIDLabel")).Text;

                        tempRowIndex++;
                    }
                    Swap("UP", Convert.ToInt32(rowIndex.ToString()), ID, swapID);
                    fillGridView(productionPlanningPriorityWCNameDropDownList.SelectedItem.ToString(), "C");

                    ((RadioButton)productionPlanningPriorityShiftCGridView.Rows[rowIndex - 1].FindControl("productionPlanningPriorityShiftCRadioButton")).Checked = true;
                    productionPlanningPriorityShiftCGridView.Rows[rowIndex - 1].BackColor = System.Drawing.Color.Yellow;
                }
            }
            else if (((ImageButton)sender).ID == "productionPlanningPriorityShiftCDownImageButton")
            {
                bool flag = false;

                if ((getRowPosition("C") == "last") || (productionPlanningPriorityShiftCGridView.Rows.Count == 0))
                {
                }
                else
                {
                    foreach (GridViewRow row in productionPlanningPriorityShiftCGridView.Rows)
                    {
                        if (flag == true)
                        {
                            tempID = ((Label)row.FindControl("productionPlanningPriorityShiftCIDLabel")).Text;
                            swapID = tempID;
                            flag = false;
                        }

                        if (((RadioButton)row.FindControl("productionPlanningPriorityShiftCRadioButton")).Checked == true)
                        {
                            rowIndex = tempRowIndex;
                            flag = true;

                            ID = ((Label)row.FindControl("productionPlanningPriorityShiftCIDLabel")).Text;
                        }
                        tempID = ((Label)row.FindControl("productionPlanningPriorityShiftCIDLabel")).Text;

                        tempRowIndex++;
                    }
                    Swap("DOWN", Convert.ToInt32(rowIndex.ToString()), ID, swapID);
                    fillGridView(productionPlanningPriorityWCNameDropDownList.SelectedItem.ToString(), "A");

                    ((RadioButton)productionPlanningPriorityShiftCGridView.Rows[rowIndex + 1].FindControl("productionPlanningPriorityShiftCRadioButton")).Checked = true;
                    productionPlanningPriorityShiftCGridView.Rows[rowIndex + 1].BackColor = System.Drawing.Color.Yellow;
                }
            }

        }

        protected void MultiViewButton_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "productionPlanningPriorityViewButton")
            {
                productionPlanningMultiView.SetActiveView(productionPlanningMultiView.Views[1]);
            }
            else if (((Button)sender).ID == "productionPlanningPlanningViewButton")
            {
                productionPlanningMultiView.SetActiveView(productionPlanningMultiView.Views[0]);
            }
        }

        protected void ShiftARadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //Clear the existing selected row

            GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((RadioButton)sender).Parent).Parent;

            foreach (GridViewRow oldrow in productionPlanningPriorityShiftAGridView.Rows)
            {
                ((RadioButton)oldrow.FindControl("productionPlanningPriorityShiftARadioButton")).Checked = false;
                oldrow.BackColor = System.Drawing.Color.White;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("productionPlanningPriorityShiftARadioButton")).Checked = true;

            row.BackColor = System.Drawing.Color.Yellow;
        }

        protected void ShiftBRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //Clear the existing selected row

            GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((RadioButton)sender).Parent).Parent;

            foreach (GridViewRow oldrow in productionPlanningPriorityShiftBGridView.Rows)
            {
                ((RadioButton)oldrow.FindControl("productionPlanningPriorityShiftBRadioButton")).Checked = false;
                oldrow.BackColor = System.Drawing.Color.White;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("productionPlanningPriorityShiftBRadioButton")).Checked = true;

            row.BackColor = System.Drawing.Color.Yellow;
        }

        protected void ShiftCRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            //Clear the existing selected row

            GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((RadioButton)sender).Parent).Parent;

            foreach (GridViewRow oldrow in productionPlanningPriorityShiftCGridView.Rows)
            {
                ((RadioButton)oldrow.FindControl("productionPlanningPriorityShiftCRadioButton")).Checked = false;
                oldrow.BackColor = System.Drawing.Color.White;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("productionPlanningPriorityShiftCRadioButton")).Checked = true;

            row.BackColor = System.Drawing.Color.Yellow;
        }

        #endregion


        #region User Defined Function

        private void fillWorkCenter()
        {

            //Description   : Function for filling productionPlanningWCDropDownList and productionPlanningPriorityWCDropDownList with WorkCenter Name
            //Author        : Brajesh kumar       || Brajesh kumar
            //Date Created  : 04-July-2011      ||
            //Date Updated  : 04-July-2011      || 09-July-2011
            //Revision No.  : 01                || 02
            //Revision Desc :                   || Fill productionPlanningPriorityWCDropDownList simultaneously

            productionPlanningWCNameDropDownList.Items.Clear();
            productionPlanningWCNameDropDownList.Items.Add("");

            productionPlanningPriorityWCNameDropDownList.Items.Clear();
            productionPlanningPriorityWCNameDropDownList.Items.Add("");

            productionPlanningWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "");
            productionPlanningWCNameDropDownList.DataBind();

            productionPlanningPriorityWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "");
            productionPlanningPriorityWCNameDropDownList.DataBind();

        }

        private void fillGridView(string wcName)
        {

            //Description   : Function for filling productionPlanningGridView
            //Author        : Brajesh kumar
            //Date Created  : 04-July-2011
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            productionPlanningGridView.DataSource = myWebService.fillGridView("Select * from vRecipeMaster WHERE wcName='" + wcName + "'", ConnectionOption.SQL);
            productionPlanningGridView.DataBind();
        }

        private void fillGridView(string wcName, string shift)
        {

            //Description   : Function for filling productionPlanningPriorityShiftAGridView , productionPlanningPriorityShift\BAGridView and productionPlanningPriorityShiftCGridView
            //Author        : Brajesh kumar
            //Date Created  : 11-July-2011
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            productionPlanningPriorityShiftAGridView.DataSource = myWebService.fillGridView("Select * from vProductionPlanning WHERE wcName='" + wcName + "' AND shift = 'A' AND dtandTime >= '" + myWebService.formatDate(productionPlanningPriorityCalenderTextBox.Text.Trim()) + "' AND dtandTime < '" + Convert.ToDateTime(myWebService.formatDate(productionPlanningPriorityCalenderTextBox.Text.Trim())).AddDays(1).ToString() + "' Order By priority", ConnectionOption.SQL);
            productionPlanningPriorityShiftAGridView.DataBind();

            productionPlanningPriorityShiftBGridView.DataSource = myWebService.fillGridView("Select * from vProductionPlanning WHERE wcName='" + wcName + "' AND shift = 'B' AND dtandTime >= '" + myWebService.formatDate(productionPlanningPriorityCalenderTextBox.Text.Trim()) + "' AND dtandTime < '" + Convert.ToDateTime(myWebService.formatDate(productionPlanningPriorityCalenderTextBox.Text.Trim())).AddDays(1).ToString() + "' Order By priority", ConnectionOption.SQL);
            productionPlanningPriorityShiftBGridView.DataBind();

            productionPlanningPriorityShiftCGridView.DataSource = myWebService.fillGridView("Select * from vProductionPlanning WHERE wcName='" + wcName + "' AND shift = 'C' AND dtandTime >= '" + myWebService.formatDate(productionPlanningPriorityCalenderTextBox.Text.Trim()) + "' AND dtandTime < '" + Convert.ToDateTime(myWebService.formatDate(productionPlanningPriorityCalenderTextBox.Text.Trim())).AddDays(1).ToString() + "' Order By priority", ConnectionOption.SQL);
            productionPlanningPriorityShiftCGridView.DataBind();
        }

        private int save(string wcID, string productTypeID, string recipeID, string quantity, string shift, string remarks)
        {
            //Description   : Function for saving and updating record in productionPlanning Table
            //Author        : Brajesh kumar
            //Date Created  : 07 July 2011
            //Date Updated  : 07 July 2011
            //Revision No.  : 01

            int flag = 0;

            if (validation(wcID, productTypeID, recipeID, shift, remarks) == false)
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET wcID = @wcID, productTypeID = @productTypeID, dtandTime = @dtandTime, recipeID = @recipeID, quantity = @quantity, shift = @shift, remarks = @remarks WHERE (wcID = @wcID) AND (productTypeID = @productTypeID) AND (recipeID = @recipeID) AND (shift = @shift)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@dtandTime", DateTime.Now);
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@quantity", Convert.ToInt32(quantity));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);
                myConnection.comm.Parameters.AddWithValue("@remarks", remarks);

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            else
            {
                if (quantity.Trim() == "")
                {
                }
                else if (Convert.ToInt32(quantity) > 0)
                {

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into productionPlanning (wcID, productTypeID, dtandTime, recipeID, quantity, shift, remarks, priority) values (@wcID, @productTypeID, @dtandTime, @recipeID, @quantity, @shift, @remarks, @priority)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", DateTime.Now);
                    myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                    myConnection.comm.Parameters.AddWithValue("@quantity", Convert.ToInt32(quantity));
                    myConnection.comm.Parameters.AddWithValue("@shift", shift);
                    myConnection.comm.Parameters.AddWithValue("@remarks", remarks);
                    myConnection.comm.Parameters.AddWithValue("@priority", 0);

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }

            return flag;
        }

        private bool validation(string wcID, string productTypeID, string recipeID, string shift, string remarks)
        {
            bool flag = true;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select * from vProduction Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND (PlanDtandTime >= @StartDtandTime) AND (PlanDtandTime < @EndDtandTime)";
            myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(productionPlanningWCIDLabel.Text.Trim()));
            myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
            myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
            myConnection.comm.Parameters.AddWithValue("@shift", shift);
            myConnection.comm.Parameters.AddWithValue("@StartDtandTime", DateTime.Today);
            myConnection.comm.Parameters.AddWithValue("@EndDtandTime", DateTime.Today.AddDays(1));

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                flag = false;
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        public string plannedQuantity(Object recipeID, Object productTypeID, Object shift)
        {
            string flag = "0";

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select PlannedQty from vProduction Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND (PlanDtandTime >= @StartDtandTime) AND (PlanDtandTime < @EndDtandTime)";
            myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(productionPlanningWCIDLabel.Text.Trim()));
            myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
            myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
            myConnection.comm.Parameters.AddWithValue("@shift", shift);
            myConnection.comm.Parameters.AddWithValue("@StartDtandTime", myWebService.formatDate(productionPlanningCalenderTextBox.Text));
            myConnection.comm.Parameters.AddWithValue("@EndDtandTime", Convert.ToDateTime(myWebService.formatDate(productionPlanningCalenderTextBox.Text)).AddDays(1));

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                flag = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        public string actualQuantity(Object recipeID, Object productTypeID, Object shift)
        {
            string flag = "0";

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select ActualQty from vProduction Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND (PlanDtandTime >= @StartDtandTime) AND (PlanDtandTime < @EndDtandTime)";
            myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(productionPlanningWCIDLabel.Text.Trim()));
            myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
            myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
            myConnection.comm.Parameters.AddWithValue("@shift", shift);
            myConnection.comm.Parameters.AddWithValue("@StartDtandTime", myWebService.formatDate(productionPlanningCalenderTextBox.Text));
            myConnection.comm.Parameters.AddWithValue("@EndDtandTime", Convert.ToDateTime(myWebService.formatDate(productionPlanningCalenderTextBox.Text)).AddDays(1));

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                flag = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        private void Swap(string option, int priority, string iD, string swapID)
        {

            //Description   : Function for swapping rows in productionPlanningPriorityShiftAGridView
            //Author        : Brajesh kumar
            //Date Created  : 12 July 2011
            //Date Updated  : 12 July 2011
            //Revision No.  : 01


            if (option == "UP")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + priority + " WHERE iD = " + iD + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);


                //Code for swaping//

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + (priority + 1) + " WHERE iD = " + swapID + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            else
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + priority + " WHERE iD = " + iD + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                //Code for swaping//

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + (priority - 1) + " WHERE iD = " + swapID + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
        }

        private void setPriority()
        {
            //Description   : Function for setting priority in productionPlanningPriorityShiftAGridView, productionPlanningPriorityShiftBGridView, productionPlanningPriorityShiftCGridView
            //Author        : Brajesh kumar
            //Date Created  : 14 July 2011
            //Date Updated  : 14 July 2011
            //Revision No.  : 01 

            foreach (GridViewRow row in productionPlanningPriorityShiftAGridView.Rows)
            {
                string id = ((Label)row.FindControl("productionPlanningPriorityShiftAIDLabel")).Text;
                string priority = ((Label)row.FindControl("productionPlanningPriorityShiftAPriorityIDLabel")).Text;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + priority + " WHERE iD = " + id + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            foreach (GridViewRow row in productionPlanningPriorityShiftBGridView.Rows)
            {
                string id = ((Label)row.FindControl("productionPlanningPriorityShiftBIDLabel")).Text;
                string priority = ((Label)row.FindControl("productionPlanningPriorityShiftBPriorityIDLabel")).Text;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + priority + " WHERE iD = " + id + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            foreach (GridViewRow row in productionPlanningPriorityShiftCGridView.Rows)
            {
                string id = ((Label)row.FindControl("productionPlanningPriorityShiftCIDLabel")).Text;
                string priority = ((Label)row.FindControl("productionPlanningPriorityShiftCPriorityIDLabel")).Text;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Update productionPlanning SET priority = " + priority + " WHERE iD = " + id + "";
                myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
        }

        public string getRowPosition(string option)
        {
            string flag = "";
            int rowIndex = 0;
            int maxRows = 0;

            if (option == "A")
            {
                maxRows = productionPlanningPriorityShiftAGridView.Rows.Count;

                foreach (GridViewRow oldrow in productionPlanningPriorityShiftAGridView.Rows)
                {
                    if (((RadioButton)oldrow.FindControl("productionPlanningPriorityShiftARadioButton")).Checked == true)
                    {
                        rowIndex = oldrow.RowIndex;
                    }
                }
            }
            else if (option == "B")
            {
                maxRows = productionPlanningPriorityShiftBGridView.Rows.Count;

                foreach (GridViewRow oldrow in productionPlanningPriorityShiftBGridView.Rows)
                {
                    if (((RadioButton)oldrow.FindControl("productionPlanningPriorityShiftBRadioButton")).Checked == true)
                    {
                        rowIndex = oldrow.RowIndex;
                    }
                }
            }
            else if (option == "C")
            {
                maxRows = productionPlanningPriorityShiftCGridView.Rows.Count;

                foreach (GridViewRow oldrow in productionPlanningPriorityShiftCGridView.Rows)
                {
                    if (((RadioButton)oldrow.FindControl("productionPlanningPriorityShiftCRadioButton")).Checked == true)
                    {
                        rowIndex = oldrow.RowIndex;
                    }
                }
            }


            if (rowIndex == 0)
            {
                flag = "first";
            }
            if (rowIndex == maxRows - 1)
            {
                flag = "last";
            }

            return flag;
        }

        #endregion
    }
}