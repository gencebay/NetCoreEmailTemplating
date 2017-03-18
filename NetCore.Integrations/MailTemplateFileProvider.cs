using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NetCore.Integrations
{
    public class MailTemplateFileChangeToken : IChangeToken
    {
        public bool ActiveChangeCallbacks => false;

        public bool HasChanged { get; set; }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return new NullDisposable();
        }

        private class NullDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }

    public class MailTemplateFileInfo : IFileInfo
    {
        private string _content;

        public bool IsDirectory { get; } = false;

        public DateTimeOffset LastModified { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }

        public string PhysicalPath { get; set; }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                Length = Encoding.UTF8.GetByteCount(Content);
            }
        }

        public bool Exists
        {
            get { return true; }
        }

        public Stream CreateReadStream()
        {
            var bytes = Encoding.UTF8.GetBytes(Content);
            return new MemoryStream(bytes);
        }
    }

    public class MailTemplateFileProvider : IFileProvider
    {
        private static string _resourceTemplatePath = "NetCore.Integrations.templates.{0}";
        private static Assembly _currentAssembly = typeof(MailTemplateFileProvider).GetTypeInfo().Assembly;

        private readonly Dictionary<string, IFileInfo> _lookup =
            new Dictionary<string, IFileInfo>(StringComparer.Ordinal);

        public MailTemplateFileProvider()
        {
            
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotSupportedException();
        }

        public MailTemplateFileInfo AddFile(string path, string contents)
        {
            var fileInfo = new MailTemplateFileInfo
            {
                Content = contents,
                Name = Path.GetFileName(path),
                LastModified = DateTime.UtcNow,
            };

            AddFile(path, fileInfo);

            return fileInfo;
        }

        public void AddFile(string path, IFileInfo contents)
        {
            _lookup[path] = contents;
        }

        public void DeleteFile(string path)
        {
            _lookup.Remove(path);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (_lookup.ContainsKey(subpath))
            {
                return _lookup[subpath];
            }
            else
            {
                var viewName = Path.GetFileName(subpath);
                var lookupPath = string.Format(_resourceTemplatePath, viewName);
                var mailTemplateFileInfo = new MailTemplateFileInfo();
                mailTemplateFileInfo.Name = viewName;
                using (Stream stream = _currentAssembly.GetManifestResourceStream(lookupPath))
                {
                    if (stream == null)
                    {
                        throw new ArgumentNullException(nameof(stream));
                    }

                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        mailTemplateFileInfo.Content = sr.ReadToEnd();
                    }
                }

                AddFile(subpath, mailTemplateFileInfo);
                return mailTemplateFileInfo;
            }
        }

        public virtual IChangeToken Watch(string filter)
        {
            return new MailTemplateFileChangeToken();
        }
    }
}
