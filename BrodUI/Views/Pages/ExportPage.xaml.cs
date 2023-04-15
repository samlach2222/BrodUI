using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using BrodUI.Helpers;
using BrodUI.Models;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using DataGrid = Wpf.Ui.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace BrodUI.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportPage : INavigableView<ViewModels.ExportViewModel>
    {
        public ViewModels.ExportViewModel ViewModel
        {
            get;
        }

        private ImageManagement Im { get; set; }

        public ExportPage(ViewModels.ExportViewModel viewModel)
        {
            ConfigManagement.SetLanguage();

            ViewModel = viewModel;

            InitializeComponent();

            if (Im == null) // if not already initialized
            {
                Im = new ImageManagement();
            }
            Im.LoadImageFromTemp();
            int width = Im.ImageWidth;
            int height = Im.ImageHeight;
            var img = Im.Image;
            System.Windows.Media.Brush[,] wireTable = ImageToDataTable.ConvertTo2dArray(img);

            // initialize DgImage
            DgImage.CanUserAddRows = false;
            DgImage.CanUserDeleteRows = false;
            DgImage.CanUserResizeColumns = false;
            DgImage.CanUserResizeRows = false;
            DgImage.CanUserSortColumns = false;
            DgImage.IsReadOnly = true;
            DgImage.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            DgImage.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            DgImage.HeadersVisibility = DataGridHeadersVisibility.None;
            DgImage.AutoGenerateColumns = true;

            DataTable dt = new DataTable();
            for (int i = 0; i < width; i++)
            {
                dt.Columns.Add(i.ToString());
            }

            for (int i = 0; i < height; i++)
            {
                dt.Rows.Add();
            }

            DgImage.ItemsSource = dt.DefaultView;
            UpdateLayout();

            DgImage.Loaded += (s, o) =>
            {
                DgImage.UpdateLayout();
                for (int j = 0; j < DgImage.Columns.Count; j++)
                {
                    for (int i = 0; i < DgImage.Items.Count - 1; i++)
                    {
                        var dataGridCellInfo = new DataGridCellInfo(
                            DgImage.Items[i], DgImage.Columns[j]);
                        var cell = DgImage
                            .ItemContainerGenerator
                            .ContainerFromItem(dataGridCellInfo.Item) as DataGridCell;
                        if (cell != null)
                        {
                            cell.Background = wireTable[j, i];
                        }
                    }
                }
            };
        }
    }
}
