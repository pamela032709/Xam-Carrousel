using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.ComponentModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;

namespace CarrouselTest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        bool _photoIsEnabled;
        private string _chosenImage = "addFoto.png";
        private string _chosenImage1 = "addFoto.png";
        private string _chosenImage2 = "addFoto.png";
        public List<MediaFile> filecollection = new List<MediaFile>();
        #region ViewModel Instance
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)

        {

            PropertyChanged?.Invoke(sender, e);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        public ObservableCollection<MyImages> Zoos { get; set; }

        public MainViewModel()
        {

            Zoos = new ObservableCollection<MyImages>
            {
                new MyImages
                {
                    ImageUrl = "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/23c1dd13-333a-459e-9e23-c3784e7cb434/2016-06-02_1049.png",

                },
                 new MyImages
                {
                    ImageUrl =    "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/6b60d27e-c1ec-4fe6-bebe-7386d545bb62/2016-06-02_1051.png",
                   
                 },
                new MyImages
                {
                    ImageUrl = "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/e8179889-8189-4acb-bac5-812611199a03/2016-06-02_1053.png",
                   
                }
            };
        }





        #region Image Selected

        public bool PhotoIsEnabled
        {
            get
            {
                return _photoIsEnabled;
            }
            set
            {
                if (_photoIsEnabled != value)
                {
                    _photoIsEnabled = value;
                    OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(PhotoIsEnabled)));


                }
            }
        }

        public string ChosenImage
        {

            get
            {
                return _chosenImage;
            }
            set
            {
                if (_chosenImage != value)
                {
                    _chosenImage = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ChosenImage)));
                }
            }
        }

        public string ChosenImage1
        {

            get
            {
                return _chosenImage1;
            }
            set
            {
                if (_chosenImage1 != value)
                {
                    _chosenImage1 = value;
                    OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ChosenImage1)));
                }
            }
        }

        public string ChosenImage2
        {

            get
            {
                return _chosenImage2;
            }
            set
            {
                if (_chosenImage2 != value)
                {
                    _chosenImage2 = value;

                    OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ChosenImage2)));

                }
            }
        }

        #endregion
       
         #region TakePicturesByCamOrAlbum
         public async void ChooseAPicture(string imageindex)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                {
                    await App.Current.MainPage.DisplayAlert("Enable Camera Access ", "Enable access so you can start taking photos ", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Camera))
                    status = results[Permission.Camera];
            }

            if (status == PermissionStatus.Granted)
            {
                var action = await App.Current.MainPage.DisplayActionSheet("", "Cancel", null, "Take a picture", "Album");


                if (action == "Take a picture")
                {

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }

                    //  int cnt = filecollection.Count;
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {

                        PhotoSize = PhotoSize.Medium,
                        Directory = "Sample",
                        Name = "myImage" + imageindex + ".jpg",


                    });

                    if (file == null)
                        return;


                    filecollection.Add(file);
                    //    cnt = filecollection.Count;
                    // Debug.Writeline("image numero "+ cnt);
                    switch (imageindex)
                    {
                        case "0": ChosenImage = file.Path; break;
                        case "1": ChosenImage1 = file.Path; break;
                        case "2": ChosenImage2 = file.Path; break;
                            //  default: ChosenImage = ChosenImage; break;
                    }

                }

                if (action == "Album")
                {

                    var PermissionForAlbum = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                    if (PermissionForAlbum != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos))
                        {
                            await Application.Current.MainPage.DisplayAlert("Please Allow Photos Access", "This will allow to share photos from your library and complete the request.", "OK");
                        }

                        var resultsAlbum = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);

                        if (resultsAlbum.ContainsKey(Permission.Photos))
                            PermissionForAlbum = resultsAlbum[Permission.Photos];
                    }
                    if (PermissionForAlbum == PermissionStatus.Granted)
                    {
                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                            return;
                        }

                        //  int cnt = filecollection.Count;
                        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {

                            PhotoSize = PhotoSize.Medium,

                        });

                        if (file == null)
                        {
                            return;

                        }

                        // CurrentFile = file;

                        filecollection.Add(file);
                        // cnt = filecollection.Count;

                        // ChosenImage = file.Path;



                        switch (imageindex)
                        {
                            case "0": ChosenImage = file.Path; break;
                            case "1": ChosenImage1 = file.Path; break;
                            case "2": ChosenImage2 = file.Path; break;
                                //  default: ChosenImage = ChosenImage; break;
                        }


                    }

                    else if (PermissionForAlbum != PermissionStatus.Unknown)
                    {
                        await Application.Current.MainPage.DisplayAlert("Please Allow Photos Access", "This will allow to share photos from your library and complete the request.", "OK");
                    }


                };

            }
            else if (status != PermissionStatus.Unknown)
            {
                await Application.Current.MainPage.DisplayAlert("Enable Camera Access ", "Enable access so you can start taking photos or sharing from the album ", "OK");


            }

        }
         #endregion

        #region TakePictureCommands
        public ICommand ImageClickedCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {

                        ChooseAPicture("0");

                    }
                    catch (Exception ex)
                    {
                      //  ExceptionHelpers.Report(ex.InnerException, "First image has been clicked and have an exeption.");
                        return;
                    }
                });
            }
        }
        public ICommand ImageClickedCommand1
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {

                        ChooseAPicture("1");

                    }
                    catch (Exception ex)
                    {
                        //ExceptionHelpers.Report(ex.InnerException, "Second image has been clicked and have an exeption.");
                        return;
                    }
                });
            }
        }
        public ICommand ImageClickedCommand2
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {

                        ChooseAPicture("2");

                    }
                    catch (Exception ex)
                    {
                       // ExceptionHelpers.Report(ex.InnerException, "Third image has been clicked and have an exeption.");
                        return;
                    }
                });
            }
        }
        #endregion


        //INIT CAROOUSEL 

        public class MyImages
        {
            public string ImageUrl { get; set; }
           
        }



    }
}