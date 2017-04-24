using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CollectionOfHelpers.GeneralExtensions
{
    /// <summary>
    /// Adds features to various WPF components that previously would have required extra logic.
    /// These are not simply readibility methods, but instead make certain tasks but faster.
    /// For example being able to expand a TreeView to a specific depth (where previously it was all or nothing)
    /// </summary>
    public static class WpfFunctionality
    {
        #region TreeView helpers
        /// <summary>
        /// TreeViewItems come with an IsExpanded property, and an ExpandSubtree() method.
        /// These allow you to expand one or all nodes on a tree. ExpandToDepth gives a middle ground
        /// where you can specify just how deep the expansion should go. 
        /// </summary>
        /// <param name="nodeToExpand"></param>
        /// <param name="maxDepth"></param>
        public static void ExpandToDepth(this TreeViewItem nodeToExpand, int maxDepth)
        {
            if (maxDepth <= 0)
            {
                return;
            }
            nodeToExpand.IsExpanded = true;

            foreach (var childNode in nodeToExpand.Items)
            {
                var expandableChild = childNode as TreeViewItem;
                expandableChild?.ExpandToDepth(maxDepth - 1);
            }
        }

        /// <summary>
        /// This expands all children of a given TreeView to a specified depth. This 
        /// method does not affect non-TreeViewItem children in the parent TreeView
        /// </summary>
        /// <param name="treeToExpand"></param>
        /// <param name="maxDepth"></param>
        public static void ExpandToDepth(this TreeView treeToExpand, int maxDepth)
        {
            foreach (var childNode in treeToExpand.Items)
            {
                var expandableChild = childNode as TreeViewItem;
                expandableChild?.ExpandToDepth(maxDepth);
            }
        }

        /// <summary>
        /// Contracts this TreeViewItem and all it's expanded children. 
        /// Opposite of ExpandToDepth.
        /// </summary>
        /// <param name="nodeToContract"></param>
        public static void ContractAll(this TreeViewItem nodeToContract)
        {
            nodeToContract.IsExpanded = false;

            foreach (var childNode in nodeToContract.Items)
            {
                var expandableChild = childNode as TreeViewItem;
                expandableChild?.ContractAll();
            }
        }

        /// <summary>
        /// Contracts this all nodes in given TreeView and all it's expanded children. 
        /// Opposite of ExpandToDepth.
        /// </summary>
        /// <param name="treeToExpand"></param>
        public static void ContractAll(this TreeView treeToExpand)
        {
            foreach (var childNode in treeToExpand.Items)
            {
                var expandableChild = childNode as TreeViewItem;
                expandableChild?.ContractAll();
            }
        }
        #endregion
    }
}
