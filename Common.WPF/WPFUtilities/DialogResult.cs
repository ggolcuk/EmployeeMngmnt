﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common.WPF.WPFUtilities
{
    public class DialogService
    {
        public DialogResult ShowDialog(string message, string caption, MessageBoxButton buttons)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);
            return ConvertToDialogResult(result);
        }

        private DialogResult ConvertToDialogResult(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.OK:
                    return DialogResult.OK;
                case MessageBoxResult.Cancel:
                    return DialogResult.Cancel;
                case MessageBoxResult.Yes:
                    return DialogResult.Yes;
                case MessageBoxResult.No:
                    return DialogResult.No;
                default:
                    return DialogResult.None;
            }
        }
    }

    public enum DialogResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }
}
