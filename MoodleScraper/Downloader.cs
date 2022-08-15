using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using ShellProgressBar;
namespace MoodleScraper
{
    public class Downloader
    {
        private static Downloader? _instance;
        public static Downloader Instance
        {
            get
            {
                return _instance = _instance ?? new Downloader();
            }
        }

        public static bool DryRun { get; set; } = false;
        private ManualResetEvent _downloadFinished = new ManualResetEvent(false);
        private Queue<DownloadCommit> _requests = new Queue<DownloadCommit>();
        private bool _routineControl = false;
        private Semaphore _maxThreads = new Semaphore(1, 1);
        private int _activeThreads = 0;
        private ProgressBar? _bar = null;

        public void AddRequest(DownloadCommit commit)
        {
            _requests.Enqueue(commit);
        }
        private void CreateDownload(DownloadCommit request)
        {
            _maxThreads.WaitOne();
            _activeThreads++;
            CreatePath(request.Path);
            var conv = FFmpeg.Conversions.New().AddParameter($" -hwaccel cuda -i {request.Link} -c copy").SetOutput(request.Path);
            ChildProgressBar? _child = null;
            if (_bar == null)
            {
                _bar = new ProgressBar(100, "", ConsoleColor.Green);
            }
            else if (_activeThreads > 1)
            {
                _child = _bar.Spawn(100, "");
            }

            conv.OnProgress += (s, e) =>
            {
                if (_activeThreads > 1 && _child != null)
                {
                    _child.Tick(e.Percent);
                    _child.Message = $"Duration: {e.Duration}";
                }
                else
                {
                    _bar.Tick(e.Percent);
                    _bar.Message = $"Download In Progress, Remaining:{_requests.Count},Active:{_activeThreads},Time:{e.Duration}";
                }

            };

            conv.Start().ContinueWith((result) =>
            {
                try
                {
                    result.Wait();
                }
                catch (Exception ex)
                {
                    _bar.WriteErrorLine($"An error occured during a download, message:{ex.Message}");
                }
                _activeThreads--;
                if (_child != null) _child.Dispose();
                _maxThreads.Release();
            }); ;
        }

        private void CreatePath(string filePath)
        {
            Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));

        }


        public Thread Start()
        {
            Thread t;
            _routineControl = true;
            if (!DryRun)
            {
                t = new Thread(() =>
                {
                    while (_routineControl)
                    {
                        _downloadFinished.Reset();
                        while (_requests.Count > 0)
                        {
                            var request = _requests.Dequeue();
                            CreateDownload(request);
                        }
                        _downloadFinished.Set();
                        Thread.Sleep(1);
                    }
                });
            }
            else
            {
                t = new Thread(() =>
                {
                    var runner = new DryRunMode();
                    while (_routineControl)
                    {
                        _downloadFinished.Reset();
                        while (_requests.Count > 0)
                        {
                            var request = _requests.Dequeue();
                            runner.CreateDownload(request);
                        }
                        _downloadFinished.Set();
                        runner.ShowInformation();
                        Thread.Sleep(1);
                    }
                });
            }
            t.Start();
            return t;
        }

        

        public void SetMaxThreads(int threads)
        {
            _maxThreads = new Semaphore(threads, threads);
        }

        public void WaitDownloads()
        {
            _downloadFinished.WaitOne();
        }
    }

    public struct DownloadCommit
    {
        internal string Link { get; private set; }
        internal string Path { get; private set; }
        private ManualResetEvent _handle = new ManualResetEvent(false);
        public bool Result { get; private set; } = false;
        public DownloadCommit(string link, string path)
        {
            Link = link;
            Path = path;
        }

        internal void Set()
        {
            Result = true;
            _handle.Set();
        }

        public void Wait()
        {
            _handle.WaitOne();
        }
    }

    internal class DryRunMode
    {
        private bool _changed = false;
        private List<DownloadCommit> _commits = new List<DownloadCommit>();
        internal void CreateDownload(DownloadCommit commit)
        {
            _commits.Add(commit);
            Directory.CreateDirectory(commit.Path.Substring(0,commit.Path.LastIndexOf("\\")));
            File.Create(commit.Path);
            _changed = true;
        }

        internal void ShowInformation()
        {
            if (_changed)
            {
                Console.Clear();
                Dictionary<string, List<string>> fileCounts = new Dictionary<string, List<string>>();
                foreach (var commit in _commits)
                {
                    var folder = commit.Path.Substring(0, commit.Path.LastIndexOf('\\'));
                    if (fileCounts.ContainsKey(folder))
                        fileCounts[folder].Add(commit.Path);
                    else
                        fileCounts[folder] = new List<string>(new string[] { commit.Link });
                }

                Console.WriteLine($"Total File Counts: {_commits.Count}");
                Console.WriteLine($"Total Estimated Space: {_commits.Count * 115}MB");
                Console.WriteLine("########### Per Folder Files");
                foreach (var key in fileCounts.Keys)
                {
                    Console.WriteLine($"{key}: Files= {fileCounts[key].Count} ");
                }
                _changed = false;
            }
        }
    }
}
