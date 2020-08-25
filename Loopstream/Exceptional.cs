using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Loopstream
{
    [Serializable]
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey,TValue}"/> class.
        /// This is the default constructor provided for XML serializer.
        /// </summary>
        public SerializableDictionary()
        {
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var pair in dictionary)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            /*if (reader.IsEmptyElement)
            {
                return;
            }*/
            var inner = reader.ReadSubtree();

            var xElement = XElement.Load(inner);
            if (xElement.HasElements)
            {
                foreach (var element in xElement.Elements())
                {
                    this.Add((TKey)Convert.ChangeType(element.Name.ToString(), typeof(TKey)), (TValue)Convert.ChangeType(element.Value, typeof(TValue)));
                }
            }

            inner.Close();

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in this.Keys)
            {
                writer.WriteStartElement(key.ToString());
                // Check to see if we can actually serialize element
                if (this[key].GetType().IsSerializable)
                {
                    // if it's Serializable doesn't mean serialization will succeed (IE. GUID and SQLError types)
                    try
                    {
                        writer.WriteValue(this[key]);
                    }
                    catch (Exception)
                    {
                        // we're not Throwing anything here, otherwise evil thing will happen
                        writer.WriteValue(this[key].ToString());
                    }
                }
                else
                {
                    // If Type has custom implementation of ToString() we'll get something useful here
                    // Otherwise we'll get Type string. (Still better than crashing).
                    writer.WriteValue(this[key].ToString());
                }
                writer.WriteEndElement();
            }
        }
    }













	[Serializable]
	public class SerializableException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializableException"/> class.
		/// Default constructor provided for XML serialization and de-serialization.
		/// </summary>
		public SerializableException()
		{
		}

		public SerializableException(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException();
			}

			this.Type = exception.GetType().ToString();

			if (exception.Data.Count != 0)
			{
				foreach (DictionaryEntry entry in exception.Data)
				{
					if (entry.Value != null)
					{
						// Assign 'Data' property only if there is at least one entry with non-null value
						if (this.Data == null)
						{
							this.Data = new SerializableDictionary<object, object>();
						}

						this.Data.Add(entry.Key, entry.Value);
					}
				}
			}

			if (exception.HelpLink != null)
			{
				this.HelpLink = exception.HelpLink;
			}

			if (exception.InnerException != null)
			{
				this.InnerException = new SerializableException(exception.InnerException);
			}

			if (exception is AggregateException)
			{
				this.InnerExceptions = new List<SerializableException>();

				foreach (var innerException in ((AggregateException)exception).InnerExceptions)
				{
					this.InnerExceptions.Add(new SerializableException(innerException));
				}

				this.InnerExceptions.RemoveAt(0);
			}

			this.Message = exception.Message != string.Empty ? exception.Message : string.Empty;

			if (exception.Source != null)
			{
				this.Source = exception.Source;
			}

			if (exception.StackTrace != null)
			{
				this.StackTrace = exception.StackTrace;
			}

			if (exception.TargetSite != null)
			{
				this.TargetSite = string.Format("{0} @ {1}", exception.TargetSite, exception.TargetSite.DeclaringType);
			}

			this.ExtendedInformation = this.GetExtendedInformation(exception);
		}

		public SerializableDictionary<object, object> Data { get; set; }

		public SerializableDictionary<string, object> ExtendedInformation { get; set; }

		public string HelpLink { get; set; }

		public SerializableException InnerException { get; set; }

		public List<SerializableException> InnerExceptions { get; set; }

		public string Message { get; set; }

		public string Source { get; set; }

		public string StackTrace { get; set; }

		// This will make TargetSite property XML serializable but RuntimeMethodInfo class does not have a parameterless
		// constructor thus the serializer throws an exception if full info is used
		public string TargetSite { get; set; }

		public string Type { get; set; }

		public override string ToString()
		{
			var serializer = new XmlSerializer(typeof(SerializableException));
			using (var stream = new MemoryStream())
			{
				stream.SetLength(0);
				serializer.Serialize(stream, this);
				stream.Position = 0;
				var doc = XDocument.Load(stream);
				return doc.Root.ToString();
			}
		}

		private SerializableDictionary<string, object> GetExtendedInformation(Exception exception)
		{
			var extendedProperties = (from property in exception.GetType().GetProperties()
			                         where
				                         property.Name != "Data" && property.Name != "InnerExceptions" && property.Name != "InnerException"
				                         && property.Name != "Message" && property.Name != "Source" && property.Name != "StackTrace"
				                         && property.Name != "TargetSite" && property.Name != "HelpLink" && property.CanRead
			                         select property).ToArray();

			if (extendedProperties.Any())
			{
				var extendedInformation = new SerializableDictionary<string, object>();

				foreach (var property in extendedProperties.Where(property => property.GetValue(exception, null) != null))
				{
					extendedInformation.Add(property.Name, property.GetValue(exception, null));
				}

				return extendedInformation;
			}
			else
			{
				return null;
			}
		}
	}












    [Serializable]
    public class GeneralInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralInfo"/> class. This is the default constructor provided for XML
        /// serialization and de-serialization.
        /// </summary>
        public GeneralInfo()
        {
        }

        [Serializable]
        class Variables
        {
            public List<string> properties, fields, keys;
            public Variables()
            {
                properties = new List<string>();
                fields = new List<string>();
                keys = new List<string>();
            }
        }

        internal GeneralInfo(SerializableException serializableException, Exception exception)
        {
            this.exception = serializableException;

            if (this.exception == null) this.exception = new SerializableException();

            HostApplicationVersion = System.Windows.Forms.Application.ProductVersion + "-" + Program.beta;

            this.CLRVersion = Environment.Version.ToString();

            this.DateTimeLC = System.DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");

            this.DateTimeUTC = System.DateTime.UtcNow.ToString("yyyy-MM-dd_HH.mm.ss");

            this.os = string.Format(
                "CLR: {1}{0}" +
                "OS: {2}{0}" +
                "i64: {3}, {4}{0}" +
                "Sysdir: {5}{0}",
                "\r\n",
                Environment.Version.ToString(),
                Environment.OSVersion.VersionString,
                Environment.Is64BitOperatingSystem,
                IntPtr.Size,
                Environment.SystemDirectory);

            if (serializableException != null)
            {
                this.ExceptionType = serializableException.Type;

                if (!string.IsNullOrEmpty(serializableException.TargetSite))
                {
                    this.TargetSite = serializableException.TargetSite;
                }
                else if (serializableException.InnerException != null && !string.IsNullOrEmpty(serializableException.InnerException.TargetSite))
                {
                    this.TargetSite = serializableException.InnerException.TargetSite;
                }

                this.ExceptionMessage = serializableException.Message;
            }

            /*Variables vars = new Variables();
            this.vars = vars;
            try
            {
                var trace = new System.Diagnostics.StackTrace(exception);
                var frame = trace.GetFrame(1);
                var methodName = frame.GetMethod().Name;
                var properties = this.GetType().GetProperties();
                var fields = this.GetType().GetFields();
                foreach (var prop in properties)
                {
                    vars.properties.Add(prop.GetValue(exception. null).ToString());
                }

            }
            catch { }*/
        }

        public string CLRVersion { get; set; }

        public string DateTimeLC { get; set; }

        public string DateTimeUTC { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets AssemblyFileVersion of host assembly.
        /// </summary>
        public string HostApplicationVersion { get; set; }

        public string TargetSite { get; set; }

        public string UserDescription { get; set; }

        public SerializableException exception { get; set; }

        //public Variables vars { get; set; }

        public string os { get; set; }

        public override string ToString()
        {
            return GeneralInfo.ser(this);
        }

        public static string ser(object o)
        {
            var serializer = new XmlSerializer(o.GetType());
            using (var stream = new MemoryStream())
            {
                stream.SetLength(0);
                serializer.Serialize(stream, o);
                stream.Position = 0;
                var doc = XDocument.Load(stream);
                return doc.Root.ToString();
            }
        }
    }
}
