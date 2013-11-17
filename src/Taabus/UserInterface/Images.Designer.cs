using System;
using System.Collections.Generic;
using System.Linq;

namespace Taabus.UserInterface
{
    sealed partial class Images
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Images));
            this._data = new System.Windows.Forms.ImageList(this.components);
            // 
            // _data
            // 
            this._data.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_data.ImageStream")));
            this._data.TransparentColor = System.Drawing.Color.Transparent;
            this._data.Images.SetKeyName(0, "none");
            this._data.Images.SetKeyName(1, "none open");
            this._data.Images.SetKeyName(2, "Syntax");
            this._data.Images.SetKeyName(3, "Symbol");
            this._data.Images.SetKeyName(4, "Context");
            this._data.Images.SetKeyName(5, "Code");
            this._data.Images.SetKeyName(6, "Pending");
            this._data.Images.SetKeyName(7, "Cache");
            this._data.Images.SetKeyName(8, "Pending Cache");
            this._data.Images.SetKeyName(9, "Ok");
            this._data.Images.SetKeyName(10, "Syntax Old");
            this._data.Images.SetKeyName(11, "Dictionary");
            this._data.Images.SetKeyName(12, "List");
            this._data.Images.SetKeyName(13, "String");
            this._data.Images.SetKeyName(14, "Question");
            this._data.Images.SetKeyName(15, "Number");
            this._data.Images.SetKeyName(16, "Type");
            this._data.Images.SetKeyName(17, "ListItem");
            this._data.Images.SetKeyName(18, "Key");
            this._data.Images.SetKeyName(19, "Bool");
            this._data.Images.SetKeyName(20, "Size");
            this._data.Images.SetKeyName(21, "CodeError");
            this._data.Images.SetKeyName(22, "Datetime");

        }

        #endregion

        System.Windows.Forms.ImageList _data;
    }
}