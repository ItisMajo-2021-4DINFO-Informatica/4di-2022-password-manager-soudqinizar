using System;
using System.IO;
using System.Text;
using Notes.Models;
using Xamarin.Forms;

namespace Notes.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class NoteEntryPage : ContentPage
    {
        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }

        public NoteEntryPage()
        {
            InitializeComponent();

            // Set the BindingContext of the page to a new Note.
            BindingContext = new Note();
        }

        void LoadNote(string filename)
        {
            try
            {
                string allText = File.ReadAllText(filename);
                string[] campi = allText.Split('§');
                // Retrieve the note and set it as the BindingContext of the page.
                Note note = new Note
                {
                    Filename = filename,
                    ServiceName = campi[0],
                    Username = campi[1],
                    Password = campi[2],
                    URL = campi[3],
                    Date = File.GetCreationTime(filename)
                };
                BindingContext = note;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load note.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (string.IsNullOrWhiteSpace(note.Filename))
            {
                // Save the file.
                var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");
                string allText = note.ServiceName + "§" +
                    note.Username + "§" +
                    note.Password + "§" +
                    note.URL;
                File.WriteAllText(filename, allText);
            }
            else
            {
                // Update the file.
                string allText = note.ServiceName + "§" +
                                note.Username + "§" +
                                note.Password + "§" +
                                note.URL;
                File.WriteAllText(note.Filename, allText);
            }

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }



        void CreatePassword(object sender, EventArgs e)
        {
            const string caratteri = "abcdefghijklmnopqursuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

            char[] generator = new char[11];
            string charSet = string.Empty;
            Random rnd = new Random();
            charSet = charSet + caratteri;

            for (int length = 0; length < 11; length++)
            {
                generator[length] = charSet[rnd.Next(charSet.Length - 1)];
            }

            password.Text = String.Join(null, generator);
        }


        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            // Delete the file.
            if (File.Exists(note.Filename))
            {
                File.Delete(note.Filename);
            }

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }
    }
}