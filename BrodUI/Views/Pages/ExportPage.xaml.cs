using BrodUI.Helpers;
using BrodUI.Models;
using System.Data;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;

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
                        // TODO : CRASH HERE, NO COLOR. 14 HOURS TO GET A CELL AND NOT BE ABLE TO CHANGE ITS COLOR...
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
