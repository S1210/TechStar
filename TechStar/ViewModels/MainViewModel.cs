using Hangfire.Annotations;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using TechStar.Commands;
using TechStar.Context;
using TechStar.Models;

namespace TechStar.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RelayCommand saveCommand;
        private Word currentWord;
        private List<Word> words;
        private string textBoxWord;

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Word> Words
        {
            get { return words; }
            set
            {
                words = value;
                OnPropertyChanged();
            }
        }
        public Word CurrentWord
        {
            get { return currentWord; }
            set
            {
                currentWord = value;
                if (CurrentWord != null)
                {
                    TextBoxWord = currentWord.Name;
                }
            }
        }
        public string TextBoxWord
        {
            get { return textBoxWord; }
            set
            {
                textBoxWord = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            GetWords();
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(obj =>
                    {
                        using (PostgersContext db = new PostgersContext())
                        {
                            Word word = new Word { Name = TextBoxWord };
                            Log log = new Log { Date = DateTime.Now };
                            db.Words.Add(word);
                            db.SaveChanges();
                            log.IDWord = word.ID;
                            db.Logs.Add(log);
                            db.SaveChanges();
                        }
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            Snackbar snackbar = obj as Snackbar;
                            var messageQueue = snackbar.MessageQueue;
                            var message = "Добавлено";
                            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                        });
                        GetWords();
                    }));
            }
        }

        private void GetWords()
        {
            using (PostgersContext db = new PostgersContext())
            {
                db.Words.Load();
                db.Logs.Load();
                Words = db.Words.ToList();
            }
        }
    }
}
