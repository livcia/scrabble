using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using CommunityToolkit.Maui.Views;

namespace randomWordGenerator
{
    public partial class SettingsPopup : Popup
    {
        public event EventHandler<double> VolumeChanged;
        public SettingsPopup(double initialVolume)
        {
            InitializeComponent();
            volumeSlider.Value = initialVolume;
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            VolumeChanged?.Invoke(this, e.NewValue);
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
