﻿using System;
using Xamarin.Forms;

namespace TaskCards.Behaviors {

	/// <summary>
	/// バインディング属性を付与するクラス
	/// </summary>
	/// <typeparam name="T">バインディング可能なオブジェクト</typeparam>
	public class InheritBindingBehavior<T> : Behavior<T> where T : BindableObject {
		private bool _inheritedBindingContedt;

		protected T AssociatedObject { get; private set; }

		protected override void OnAttachedTo(T bindableObject) {
			base.OnAttachedTo(bindableObject);

			AssociatedObject = bindableObject;
			InheritBindingContext(bindableObject);

			bindableObject.BindingContextChanged += OnBindingContextChanged;
		}

		private void OnBindingContextChanged(object sender, EventArgs e) {
			InheritBindingContext(AssociatedObject);
		}

		private void InheritBindingContext(T bindableObject) {
			if (BindingContext == null || _inheritedBindingContedt) {
				BindingContext = bindableObject.BindingContext;
				_inheritedBindingContedt = true;
			}
		}

		protected override void OnDetachingFrom(T bindableObject) {
			BindingContext = null;
			bindableObject.BindingContextChanged -= OnBindingContextChanged;
		}
	}
}
