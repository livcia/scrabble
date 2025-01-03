﻿using CommunityToolkit.Maui.Views;
using randomWordGenerator.Game;

namespace randomWordGenerator;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        media_Element.Play(); //don't work
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage());
    }

    private void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        var popup = new SettingsPopup(media_Element.Volume);
        popup.VolumeChanged += OnVolumeChanged;
        this.ShowPopup(popup);
    }

    private void OnVolumeChanged(object sender, double newVolume)
    {
        media_Element.Volume = newVolume;
    }

    private void ChangeMusic(object sender, EventArgs e)
    {
        if (media_Element.CurrentState.ToString() == "Playing")
        {
            media_Element.Pause();
            MusicIcon.Source = "mute.png";
        }
        else
        {
            media_Element.Play();
            MusicIcon.Source = "unmute.png";
        }
    }
}