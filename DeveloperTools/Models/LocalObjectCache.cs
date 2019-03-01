using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DeveloperTools.Models
{
	public class LocalObjectCache
	{
		public IEnumerable<DictionaryEntry> CachedItems { get; set; }

		public string FilteredBy { get; set; }

		public IEnumerable<SelectListItem> Choices { get; set; }
	}
}