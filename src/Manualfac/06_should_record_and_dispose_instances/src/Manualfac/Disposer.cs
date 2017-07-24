using System;
using System.Collections.Generic;

namespace Manualfac
{
    class Disposer : Disposable
    {
        #region Please implements the following methods

        Stack<IDisposable> items = new Stack<IDisposable>();

        /*
         * The disposer is used for disposing all disposable items added when it is disposed.
         */

        public void AddItemsToDispose(object item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));

            items.Push((IDisposable)item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (items.Count > 0)
                {
                    IDisposable item = items.Pop();
                    item.Dispose();
                }
                items = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}