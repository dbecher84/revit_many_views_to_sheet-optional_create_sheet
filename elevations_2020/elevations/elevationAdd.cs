using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters;

namespace elevationsToSheets
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class elevationAdd : IExternalCommand
    {

        public XYZ Usepoint(ViewSheet sheetpoint)
        {
            BoundingBoxUV uv = sheetpoint.Outline;
            ////these will place the views in the center of the sheet Min is lower left corner Max is upper right corner.
            ////Min/Max.U is the x value Min/Max.V is y.
            //double xx = (uv.Max.U + uv.Min.U) / 2;
            //double yy = (uv.Max.V + uv.Min.V) / 2;
            double xx = uv.Min.V;
            double yy = uv.Max.V;
            XYZ point = new XYZ(xx, yy + .5, 0);
            return point;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get application and documnet objects

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            ///////////////////////////////////////////////////////////////
            
            //collect titleblocks in revit
            FilteredElementCollector titleblockcol = new FilteredElementCollector(doc);
            titleblockcol.OfClass(typeof(FamilySymbol));
            titleblockcol.OfCategory(BuiltInCategory.OST_TitleBlocks);

            /////////////////////////////////////////////////////////////////////

            //collect sheets in revit
            FilteredElementCollector sheetCollector = new FilteredElementCollector(doc);
            sheetCollector.OfClass(typeof(ViewSheet));


            //collect views in revit
            FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
            viewCollector.OfClass(typeof(View));

            ////////////////////////////////////////////////////////////////////////

            //start transaction
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("transaction");

            ///////////////////////////////////////////////////////////////////

                //ask if a new sheet needs to be created for views
                var newsheet = new sheetcreation.createSheet();
                newsheet.ShowDialog();

                //////////////////////////////////////////////////////////////////////


                if (newsheet.createNewSheet == "yes")
                {
                    //list of titleblocks to send to the windows form 
                    List<string> tblockList = new List<string>();
                    foreach (FamilySymbol tblock in titleblockcol)
                    {
                        tblockList.Add(tblock.Name);
                    }

                    //initate list of titleblocks form
                    var t = new elevations_tblocks.titleblockform(tblockList);
                    t.ShowDialog();

                    if (t.tblockToUse.Count() == 1)
                    {
                        if (t.cleanNum != "")
                        {
                            foreach (FamilySymbol FS in titleblockcol)
                            {
                                if (FS.Name == t.tblockToUse.ElementAt(0))
                                {
                                    ViewSheet NewSheet = ViewSheet.Create(doc, FS.Id);
                                    NewSheet.Name = t.cleanName;
                                    NewSheet.SheetNumber = t.cleanNum;
                                }
                            }
                        }
                        else
                        {
                            TaskDialog.Show("sheet issue :) ", "A sheet number must be entered");
                            t.ShowDialog();
                        }
                    }
                    else
                    {
                        TaskDialog.Show("sheet issue :) ", "A title block must be selected");
                        t.ShowDialog();
                    } 
                }
               
                /////////////////////////////////////////////////////////////////

                //list of sheets to send to the windows form viewList
                List<string> listSheetNums = new List<string>();
                foreach (ViewSheet sheet in sheetCollector)
                {
                    listSheetNums.Add(sheet.SheetNumber);
                }

                //initate list of sheets form
                var l = new get_sheet.viewsToSheet(listSheetNums);
                l.ShowDialog();

                ////////////////////////////////////////////////////////////////////

                //Select views to place on sheets
                IList<Reference> pickedObjs = uidoc.Selection.PickObjects(ObjectType.Element, "Select elements");
                List<ElementId> ids = (from Reference r in pickedObjs select r.ElementId).ToList();

                ///////////////////////////////////////////////////////////////////

                //get selected element names
                List<string> sbname = new List<string>();
                //List<string> sbids = new List<string>();
                //tx.Start("transaction");
                if (pickedObjs != null && pickedObjs.Count > 0)
                {
                    foreach (ElementId eid in ids)
                    {
                        Element e = doc.GetElement(eid);
                        //sbids.Add(eid.ToString());
                        sbname.Add(e.Name);
                        //TaskDialog.Show("selected name:) ", e.Name);
                        //TaskDialog.Show("name list count:) ", sbname.Count().ToString());
                        //TaskDialog.Show("selected ID :) ", sbids.ToString());
                    }

                 ////////////////////////////////////////////////////////////////

                    //get sheet for view placement
                    ElementId sheetUsed = null;
                    ViewSheet sheetUsedId = null;
                    if (l.sheetForViews.Count() == 0)
                    {
                        TaskDialog.Show("sheet issue :) ", "No sheet selected");
                        l.ShowDialog();
                    }
                    if (l.sheetForViews.Count() > 1)
                    {
                        TaskDialog.Show("sheet issue :) ", "Too many sheets selected");
                        l.ShowDialog();
                    }
                    if (l.sheetForViews.Count() == 1)
                    foreach (ViewSheet su in sheetCollector)
                        {
                        if (su.SheetNumber.ToString() == l.sheetForViews.ElementAt(0))
                        {
                            sheetUsed = su.Id;
                            sheetUsedId = su;
                            //TaskDialog.Show("sheet Used :) ", su.Id.ToString() +" " + "added to use list");
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////////

                    //place vies on sheet

                    XYZ StartPoint = Usepoint(sheetUsedId);

                    XYZ countx = new XYZ(1, 0, 0);

                    List<string> viewsUsed = new List<string>();
                    foreach (string pickedView in sbname)
                        foreach (View vsel in viewCollector)
                            if (vsel.Name.ToString() == pickedView)
                            {
                                ElementId el = vsel.Id;
                                if (Viewport.CanAddViewToSheet(doc, sheetUsed, el) == false)
                                {
                                    TaskDialog.Show("viewport issue :) ", "Can't place view" + " " + vsel.Name + " " + "on sheet. Check view is not already on sheet.");
                                }
                                else
                                {
                                    //TaskDialog.Show("View Id being used :) ", pickedView.ToString() + " " + "Id send to create method");
                                    Viewport.Create(doc, sheetUsed, el, StartPoint);
                                    StartPoint += countx;

                                }
                            }
                }
                tx.Commit();
            }
            return Result.Succeeded;
        }
    }
}