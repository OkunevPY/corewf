//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

namespace CoreWf
{
    using CoreWf.Runtime;
    using System.Runtime;
    using System.Transactions;

    [Fx.Tag.XamlVisible(false)]
    public sealed class NativeActivityTransactionContext : NativeActivityContext
    {
        ActivityExecutor executor;
        RuntimeTransactionHandle transactionHandle;

        internal NativeActivityTransactionContext(ActivityInstance instance, ActivityExecutor executor, BookmarkManager bookmarks, RuntimeTransactionHandle handle)
            : base(instance, executor, bookmarks)
        {
            this.executor = executor;
            this.transactionHandle = handle;
        }

        public void SetRuntimeTransaction(Transaction transaction)
        {
            ThrowIfDisposed();

            if (transaction == null)
            {
                throw CoreWf.Internals.FxTrace.Exception.ArgumentNull("transaction");
            }

            this.executor.SetTransaction(this.transactionHandle, transaction, transactionHandle.Owner, this.CurrentInstance);
        }
    }
}
