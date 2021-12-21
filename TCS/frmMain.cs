using Com.Hd.Common.View.DevExpressTemplate;
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Hmx.DHAKA.TCS.Environment.Item;
using Hmx.DHAKA.TCS.Environment.Service;
using Hmx.DHAKA.TCS.Common;
using CommonClass.Database.Mapper;
using DevExpress.Xpf.Core;
using DevExpress.XtraBars;
using DevExpress.LookAndFeel;
using HitopsCommon;
using DevExpress.XtraSplashScreen;
using Hitops.exception;
using Hmx.Skit.SunkwangApp;

namespace Hmx.DHAKA.TCS
{
    public partial class frmMain : BaseDevRibbonForm
    {
        #region FIELD AREA
        
        #endregion
        #region PROPERTY AREA
        #endregion
        #region INITIAL AREA
        public frmMain()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.frmMain_Load);
        }
        #endregion
        #region EVENT AREA
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.ribbonControl.ApplicationButtonDropDownControl = this.backstageViewControl1;
            this.backstageViewControl1.OwnerControl = this.ribbonControl;

            this.AddThemaList();

            WindowsFormsSettings.FormThickBorder = false;
            WindowsFormsSettings.MdiFormThickBorder = true;
            WindowsFormsSettings.ThickBorderWidth = 5;

            UserLookAndFeel.Default.SetSkinStyle(RegistryFunc.getKey("Thema"));


            this.AddEventHandler();
        }
        #endregion
        #region METHOD AREA
        private void AddEventHandler()
        {
            #region Control
            this.btnWindowCascade.ItemClick += new ItemClickEventHandler(this.WindowAlignment_ItemClick);
            this.btnWindowTileHorizontal.ItemClick += new ItemClickEventHandler(this.WindowAlignment_ItemClick);
            this.btnWindowTileVertical.ItemClick += new ItemClickEventHandler(this.WindowAlignment_ItemClick);
            #endregion
            #region Menu
            //Registration
            this.btnRegistrationAgent.ItemClick += new ItemClickEventHandler(this.RibbonBarButton_ItemClick); // Registartin Agent
            this.btnAgentList.ItemClick += new ItemClickEventHandler(this.RibbonBarButton_ItemClick); // Agent List
            #endregion
        }
        /// <summary>
        /// 버늩이름으로 폼이름을 찾아서띄움
        /// /// ex) btnFormName ==> frmFormName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            RibbonBarManager manager = sender as RibbonBarManager;
            BarButtonItem buttonItem = e.Item as BarButtonItem;
            if (manager == null || buttonItem == null) return;

            try
            {
                // Open Form
                string name = buttonItem.Name.Substring(3);

                if (CommFunc.checkMenuAuthority(Program.g_MenuPrefix + name, CommFunc.gloTMLCod) == false)
                {
                    MessageBox.Show("Authority Access Error");
                    return;
                }

                OpenForm("frm" + name);
            }
            catch (HMMException ex)
            {
                CommFunc.ShowExceptionBox(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static Form GetAssemblyForm(string formName)
        {
            Form form = null;

            foreach (Type t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.Name == formName)
                {
                    object o = Activator.CreateInstance(t);                    
                    form = o as Form;
                    form.Name = t.Name;
                    break;
                }
            }

            return form;
        }

        private void OpenForm(string formName)
        {
            try
            {
                // Show Wait SplashScreen
                SplashScreenManager.ShowForm(this, typeof(frmWait), true, true, false);

                // Open Form
                Form selectedForm = GetAssemblyForm(formName);

                CommFunc.CheckLoadForm(selectedForm);

                // Close Wait SplashScreen
                SplashScreenManager.CloseForm(false);
            }
            catch (HMMException ex)
            {
                CommFunc.ShowExceptionBox(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void WindowAlignment_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem buttonItem = e.Item as BarButtonItem;

            if (buttonItem.Equals(this.btnWindowCascade) == true)
            {
                this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
            }
            else if (buttonItem.Equals(this.btnWindowTileHorizontal) == true)
            {
                this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
            }
            else if (buttonItem.Equals(this.btnWindowTileVertical) == true)
            {
                this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
            }
        }
        private void AddThemaList()
        {
            ThemaService TmService = new ThemaService();
            IList<ThemaItem> items = TmService.Inquiry();
            DataTable dtThema = BindDB2Class.BindDatatable(items);

            string[] themaList = DataTableController.GetList(dtThema, "KEY");

            List<GalleryItem> galleryItemList = new List<GalleryItem>();
            
            // Find Specific Item
            for (int i = 0; i < srgThema.Gallery.Groups.Count; i++)
            {
                GalleryItemGroup group = srgThema.Gallery.Groups[i] as GalleryItemGroup;

                for (int j = 0; j < group.Items.Count; j++)
                {
                    GalleryItem item = group.Items[j] as GalleryItem;

                    foreach (string skin in themaList)
                    {
                        if (item.Caption.Contains(skin))
                        {
                            galleryItemList.Add(item);
                            //item.Visible = true;
                        }
                    }
                }
            }

            // Remove All
            srgThema.Gallery.Groups.Clear();

            // Thema List Sort
            List<GalleryItem> sortedThemaList = new List<GalleryItem>();

            foreach (string skin in themaList)
            {
                foreach (GalleryItem item in galleryItemList)
                {
                    if (item.Caption.Contains(skin))
                    {
                        sortedThemaList.Add(item);
                    }
                }
            }

            // Add Group
            GalleryItemGroup newGroup = new GalleryItemGroup();
            foreach (GalleryItem item in sortedThemaList)
            {
                newGroup.Items.Add(item);
            }

            //Set Thema List
            srgThema.Gallery.Groups.Add(newGroup);

            // Rename
            foreach (var item in srgThema.Gallery.GetAllItems())
            {
                item.Caption = item.Hint = DataTableController.findValue(dtThema, "KEY", item.Caption, "VALUE");
                item.ItemClick += new GalleryItemClickEventHandler(this.Thema_Click);
            }
        }
        private void Thema_Click(object sender, GalleryItemClickEventArgs e)
        {
            RegistryFunc.setKey("Thema", e.Item.Value.ToString());
        }
        #endregion


        //private void ShowNewForm(object sender, EventArgs e)
        //{
        //    Form childForm = new Form();
        //    childForm.MdiParent = this;
        //    childForm.Show();
        //}

        //private void OpenFile(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    openFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
        //    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        string FileName = openFileDialog.FileName;
        //    }
        //}

        //private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog();
        //    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    saveFileDialog.Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*";
        //    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        string FileName = saveFileDialog.FileName;
        //    }
        //}
    }
}
