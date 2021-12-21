using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HitopsCommon
{
    public class BaseItem : INotifyPropertyChanged, ICloneable
    {
        #region INITIALIZE
        public BaseItem()
        {
            this._guid = Guid.NewGuid();
            this.OpCode = OpCodes.None;
            this.PropertyChanged += new PropertyChangedEventHandler(BaseItem_PropertyChanged);
        }
        #endregion

        #region FIELDS
        private bool _rowLock;
        private string _cellLockFieldList;
        private Guid _guid = new Guid();
        #endregion

        #region ENUM
        public enum OpCodes
        {
            None = 0,
            Read = 1,
            Create = 2,
            Update = 4,
            Delete = 8
        }
        #endregion

        #region PROPERTIES
        public bool RowLock
        {
            get { return this._rowLock; }
            set { this._rowLock = value; }
        }

        public string CellLockFieldList
        {
            get { return this._cellLockFieldList; }
            set { this._cellLockFieldList = value; }
        }

        public OpCodes OpCode { get; set; }

        public BaseItem BackupItem { get; set; }

        public Guid GUID
        {
            get { return this._guid; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region METHODS
        public virtual void MakeBackupItem()
        {
            this.BackupItem = null;
            this.BackupItem = this.MemberwiseClone() as BaseItem;
        }

        public virtual void MakeBackupItem(bool makeRead)
        {
            this.MakeBackupItem();
            if (makeRead == true) this.OpCode = OpCodes.Read;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool LockOpCode { get; set; }
        #endregion

        #region EVENT
        private void BaseItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;

            PropertyInfo changedProperty = this.GetType().GetProperty(propertyName);

            if (changedProperty != null && this.OpCode == OpCodes.Read
                && this.LockOpCode == false)
            {
                this.OpCode = OpCodes.Update;
            }
        }
        #endregion
    }
}
