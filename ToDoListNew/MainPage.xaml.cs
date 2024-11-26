using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ToDoListNew
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
    public class TaskItem
    {
        public string Description { get; set; }
        public bool IsCompletedCheck { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _newTaskText;
        private bool _isCompleted;

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }
        public string NewTaskText
        {
            get => _newTaskText;
            set
            {
                _newTaskText = value;
                OnPropertyChanged();
                (AddTaskCommand as Command).ChangeCanExecute();
            }
        }

        public ObservableCollection<TaskItem> Tasks { get; set; }

        public Command AddTaskCommand { get; }
        public Command DeleteTaskCommand { get; }
        public Command TaskCompletedCommand {  get; }

        public MainViewModel()
        {
            Tasks = new ObservableCollection<TaskItem>();

            AddTaskCommand = new Command(() =>
            {
                AddTask();
            }, () => !string.IsNullOrWhiteSpace(NewTaskText));

            DeleteTaskCommand = new Command<TaskItem>(DeleteTask);
            TaskCompletedCommand = new Command<TaskItem>(TaskCompleted);
        }

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTaskText))
            {
                Tasks.Add(new TaskItem { Description = NewTaskText });
                NewTaskText = string.Empty;
            }
        }

        private void DeleteTask(TaskItem task)
        {
            if (task != null)
            {
                Tasks.Remove(task);
            }
        }

        private void TaskCompleted(TaskItem task)
        {
            if (task != null) 
            {
                task.IsCompletedCheck = !task.IsCompletedCheck;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
