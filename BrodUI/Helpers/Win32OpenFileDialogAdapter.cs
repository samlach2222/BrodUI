using Microsoft.Win32;
using System.Collections.Generic;
using static BrodUI.Models.ImageManagement;

namespace BrodUI.Helpers
{
    /// <summary>
    /// Adapter for the Win32 OpenFileDialog
    /// </summary>
    public class Win32OpenFileDialogAdapter : IOpenFileDialog
    {
        /// <summary>
        /// Name of the file
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// If the dialog result is true or false
        /// </summary>
        public bool? Result { get; set; }

        /// <summary>
        /// Name of the files
        /// </summary>
        public string[]? FileNames { get; }

        /// <summary>
        /// Filter for the dialog (image filter)
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Function to show the dialog
        /// </summary>
        /// <returns>if it's ok or not</returns>
        public bool? ShowDialog()
        {
            OpenFileDialog dialog = new()
            {
                Filter = Filter
            };

            if (Result != null)
            {
                dialog.CustomPlaces = new List<FileDialogCustomPlace>
                {
                    new(FileName)
                };
            }

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                FileName = dialog.FileName;
                return true;
            }
            else
            {
                FileName = null;
                return false;
            }
        }
    }
}

