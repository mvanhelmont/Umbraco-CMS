﻿using System.Runtime.Serialization;
using Umbraco.Core.IO;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Web.Models.ContentEditing;

namespace Umbraco.Web.Models.Trees
{
    /// <summary>
    /// Represents a model in the tree
    /// </summary>
    /// <remarks>
    /// TreeNode is sealed to prevent developers from adding additional json data to the response
    /// </remarks>
    [DataContract(Name = "node", Namespace = "")]
    public class TreeNode : EntityBasic
    {
        /// <summary>
        /// Internal constructor, to create a tree node use the CreateTreeNode methods of the TreeApiController.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="getChildNodesUrl"></param>
        /// <param name="menuUrl"></param>
        internal TreeNode(string nodeId, string getChildNodesUrl, string menuUrl)
        {
            Id = nodeId;
            ChildNodesUrl = getChildNodesUrl;
            MenuUrl = menuUrl;
            CssClasses = new List<string>();
             //default
            Icon = "icon-folder-close";
        }
        
        /// <summary>
        /// A flag to set whether or not this node has children
        /// </summary>
        [DataMember(Name = "hasChildren")]
        public bool HasChildren { get; set; }
        
        /// <summary>
        /// The tree nodetype which refers to the type of node rendered in the tree
        /// </summary>
        [DataMember(Name = "nodeType")]
        public string NodeType { get; set; }
        
        /// <summary>
        /// Optional: The Route path for the editor for this node
        /// </summary>
        /// <remarks>
        /// If this is not set, then the route path will be automatically determined by: {section}/edit/{id}
        /// </remarks>
        [DataMember(Name = "routePath")]
        public string RoutePath { get; set; }

        /// <summary>
        /// The JSON url to load the nodes children
        /// </summary>
        [DataMember(Name = "childNodesUrl")]
        public string ChildNodesUrl { get; set; }

        /// <summary>
        /// The JSON url to load the menu from
        /// </summary>
        [DataMember(Name = "menuUrl")]
        public string MenuUrl { get; set; }
        
        /// <summary>
        /// The icon to use for the node, this can be either a path to an image or a Css class. 
        /// If a '/' is found in the string then it will be considered a path to an image.
        /// </summary>
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Returns true if the icon represents a CSS class instead of a file path
        /// </summary>
        [DataMember(Name = "iconIsClass")]
        public bool IconIsClass
        {
            get
            {
                if (Icon.IsNullOrWhiteSpace())
                {
                    return true;
                }

                if (Icon.StartsWith(".."))
                    return false;


                //if it starts with a '.' or doesn't contain a '.' at all then it is a class
                return Icon.StartsWith(".") || Icon.Contains(".") == false;
            }
        }

        /// <summary>
        /// Returns the icon file path if the icon is not a class, otherwise returns an empty string
        /// </summary>
        [DataMember(Name = "iconFilePath")]
        public string IconFilePath
        {
            get
            {
                if (IconIsClass)
                    return string.Empty;

                //absolute path with or without tilde
                if (Icon.StartsWith("~") || Icon.StartsWith("/"))
                    return IOHelper.ResolveUrl("~" + Icon.TrimStart('~'));

                //legacy icon path
                return IOHelper.ResolveUrl("~/umbraco/images/umbraco/" + Icon);
            }
        }

        /// <summary>
        /// A list of additional/custom css classes to assign to the node
        /// </summary>
        [DataMember(Name = "cssClasses")]
        public IList<string> CssClasses { get; private set; }
    }

}
