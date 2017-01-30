//
//  Copyright (C) 2017 Fluendo S.A.
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using VAS.Core.Common;
using VAS.Core.Interfaces.MVVMC;
using VAS.Core.License;
using VAS.Core.Store;

namespace VAS.Core.MVVMC
{
	public class LimitedCollectionViewModel<TModel, TViewModel, TLimitation> : CollectionViewModel<TModel, TViewModel>
		where TViewModel : IViewModel<TModel>, new()
		where TLimitation : LicenseLimitation
	{
		TLimitation limitation;
		bool sortByCreationDateDesc;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VAS.Core.MVVMC.LimitedCollectionViewModel`3"/> class.
		/// </summary>
		/// <param name="sortByCreationDateDesc">If set to <c>true</c> sort by creation date desc.</param>
		public LimitedCollectionViewModel (bool sortByCreationDateDesc = true)
		{
			this.sortByCreationDateDesc = sortByCreationDateDesc;
		}

		/// <summary>
		/// Gets or sets the limitation.
		/// </summary>
		/// <value>The limitation.</value>
		public TLimitation Limitation {
			get {
				return limitation;
			}
			set {
				if (limitation != null) {
					limitation.PropertyChanged -= LimitationPropertyChanged;
				}
				limitation = value;
				if (limitation != null) {
					limitation.PropertyChanged += LimitationPropertyChanged;
				}
				ApplyLimitation ();
			}
		}

		/// <summary>
		/// Gets the collection of child ViewModel
		/// </summary>
		/// <value>The ViewModels collection.</value>
		public RangeObservableCollection<TViewModel> FullViewModels {
			private set;
			get;
		} = new RangeObservableCollection<TViewModel> ();

		protected virtual void ApplyLimitation ()
		{
			ViewModels.Clear ();
			if (Limitation == null || Limitation.Enabled == false) {
				if (typeof (StorableBase).IsAssignableFrom (typeof (TModel))) {
					ViewModels.AddRange (
						FullViewModels.Sort ((vm) => (vm.Model as StorableBase)?.CreationDate, sortByCreationDateDesc));
				} else {
					ViewModels.AddRange (FullViewModels);
				}
			} else {
				if (typeof (StorableBase).IsAssignableFrom (typeof (TModel))) {
					ViewModels.AddRange (
						FullViewModels.Sort ((vm) => (vm.Model as StorableBase)?.CreationDate, sortByCreationDateDesc).Take (Limitation.Maximum));
				} else {
					ViewModels.AddRange (FullViewModels.Take (Limitation.Maximum));
				}
			}
		}

		protected override void SetModel (RangeObservableCollection<TModel> model)
		{
			if (FullViewModels != null) {
				FullViewModels.CollectionChanged -= HandleViewModelsCollectionChanged;
			}
			if (this.model != null) {
				this.model.CollectionChanged -= HandleModelsCollectionChanged;
			}
			FullViewModels.Clear ();
			modelToViewModel = new Dictionary<TModel, TViewModel> ();
			this.model = model;
			AddViewModels (0, this.model);
			if (FullViewModels != null) {
				FullViewModels.CollectionChanged += HandleViewModelsCollectionChanged;
			}
			if (this.model != null) {
				this.model.CollectionChanged += HandleModelsCollectionChanged;
			}
		}

		protected override void HandleModelsCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (editing) {
				return;
			}
			editing = true;
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				AddViewModels (e.NewStartingIndex, e.NewItems.OfType<TModel> ());
				break;
			case NotifyCollectionChangedAction.Remove:
				FullViewModels.RemoveRange (e.OldItems.OfType<TModel> ().Select ((arg) => modelToViewModel [arg]));
				foreach (TModel model in e.OldItems) {
					modelToViewModel.Remove (model);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				FullViewModels.Clear ();
				modelToViewModel.Clear ();
				break;
			}
			ApplyLimitation ();
			editing = false;
		}

		protected override void AddViewModels (int index, IEnumerable<TModel> models)
		{
			if (models == null) {
				return;
			}

			var viewModels = new List<TViewModel> ();
			foreach (TModel tModel in models) {
				var viewModel = CreateInstance (tModel);
				viewModels.Add (viewModel);
				modelToViewModel [tModel] = viewModel;
			}
			FullViewModels.InsertRange (index, viewModels);
		}

		void LimitationPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Enabled" || e.PropertyName == "Maximum") {
				ApplyLimitation ();
			}
		}
	}
}
