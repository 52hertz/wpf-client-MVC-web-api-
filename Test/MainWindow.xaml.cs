using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string DefaultPathToFileForTextBlock => "none";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectImage_Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                PathToFile_Run.Text = openFileDialog.FileName;
            }
            else
            {
                PathToFile_Run.Text = DefaultPathToFileForTextBlock;
            }
        }

        private async void DoUploadOperation_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUri_TextBox.Text),             
            })
            {
                httpClient.DefaultRequestHeaders.Add("fileName", Path.GetFileName(PathToFile_Run.Text));

                using (var postData = new ByteArrayContent(File.ReadAllBytes(PathToFile_Run.Text)))
                {
                    var httpResponse = await httpClient.PostAsync("api/image/post", postData);

                    var resultMessage = default(string);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        resultMessage = "Success!";
                    }
                    else
                    {
                        resultMessage = "Fail...";
                    }
                    MessageBox.Show(resultMessage);
                }
            }
        }

        private void BaseUri_TextBox_GotMouseCapture(object sender, MouseEventArgs e) =>
            BaseUri_TextBox.Select(0, BaseUri_TextBox.Text.Length);
    }
}
