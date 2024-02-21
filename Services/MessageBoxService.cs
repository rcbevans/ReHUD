using ReHUD.Interfaces;
using System.Windows;

namespace ReHUD.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public Task<bool> ShowOkCancelDialog(string title, string message, MessageBoxType? type) {
            var image = type switch {
                null => MessageBoxImage.None,
                MessageBoxType.none => MessageBoxImage.None,
                MessageBoxType.info => MessageBoxImage.Information,
                MessageBoxType.warning => MessageBoxImage.Warning,
                MessageBoxType.error => MessageBoxImage.Error,
                _ => MessageBoxImage.None,
            };
            return Task.Run(() => {
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OKCancel, image);
                return result switch {
                    MessageBoxResult.OK => true,
                    MessageBoxResult.Cancel => false,
                    _ => false,
                };
            });
        }

        public Task<bool> ShowYesNoDialog(string title, string message, MessageBoxType? type) {
            var image = type switch {
                null => MessageBoxImage.None,
                MessageBoxType.none => MessageBoxImage.None,
                MessageBoxType.info => MessageBoxImage.Information,
                MessageBoxType.warning => MessageBoxImage.Warning,
                MessageBoxType.error => MessageBoxImage.Error,
                _ => MessageBoxImage.None,
            };
            return Task.Run(() => {
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, image);
                return result switch {
                    MessageBoxResult.Yes => true,
                    MessageBoxResult.No => false,
                    _ => false,
                };
            });
        }
    }
}
