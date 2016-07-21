﻿//---------------------------------------------------------------------
// <copyright file="CollectionNavigationNode.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.OData.Edm;
using Microsoft.OData.Metadata;
using ODataErrorStrings = Microsoft.OData.Strings;

namespace Microsoft.OData.UriParser
{
    /// <summary>
    /// Query node representing a collection navigation property.
    /// </summary>
    public sealed class CollectionNavigationNode : EntityCollectionNode
    {
        /// <summary>
        /// The navigation property of the single entity this node represents.
        /// </summary>
        private readonly IEdmNavigationProperty navigationProperty;

        /// <summary>
        /// The resource type of a single entity item from the collection represented by this node.
        /// </summary>
        private readonly IEdmEntityTypeReference edmEntityTypeReference;

        /// <summary>
        /// The type of the collection represented by this node.
        /// </summary>
        private readonly IEdmCollectionTypeReference collectionTypeReference;

        /// <summary>
        /// The parent node.
        /// </summary>
        private readonly SingleResourceNode source;

        /// <summary>
        /// The navigation source from which the collection of entities comes from.
        /// </summary>
        private readonly IEdmNavigationSource navigationSource;

        /// <summary>
        /// The parsed segments in the path.
        /// </summary>
        private readonly List<ODataPathSegment> parsedSegments;

        /// <summary>
        /// Creates a CollectionNavigationNode.
        /// </summary>
        /// <param name="source">The parent of this collection navigation node.</param>
        /// <param name="navigationProperty">The navigation property that defines the collection node.</param>
        /// <param name="bindingPath">The binding path of navigation property</param>
        /// <returns>The collection node.</returns>
        /// <exception cref="System.ArgumentNullException">Throws if the input source or navigation property is null.</exception>
        /// <exception cref="ArgumentException">Throws if the input navigation doesn't target a collection.</exception>
        public CollectionNavigationNode(SingleResourceNode source, IEdmNavigationProperty navigationProperty, IEdmPathExpression bindingPath)
            : this(navigationProperty)
        {
            ExceptionUtils.CheckArgumentNotNull(source, "source");
            this.source = source;

            this.navigationSource = source.NavigationSource != null ? source.NavigationSource.FindNavigationTarget(navigationProperty, bindingPath) : null;
        }

        /// <summary>
        /// Creates a CollectionNavigationNode.
        /// </summary>
        /// <param name="source">The navigation source.</param>
        /// <param name="navigationProperty">The navigation property that defines the collection node.</param>
        /// <param name="bindingPath">The binding path of navigation property</param>
        /// <returns>The collection node.</returns>
        /// <exception cref="System.ArgumentNullException">Throws if the input navigation property is null.</exception>
        /// <exception cref="ArgumentException">Throws if the input navigation doesn't target a collection.</exception>
        internal CollectionNavigationNode(IEdmNavigationSource source, IEdmNavigationProperty navigationProperty, IEdmPathExpression bindingPath)
            : this(navigationProperty)
        {
            this.navigationSource = source != null ? source.FindNavigationTarget(navigationProperty, bindingPath) : null;
        }

        /// <summary>
        /// Constructs a CollectionNavigationNode.
        /// </summary>
        /// <param name="source">he previous node in the path.</param>
        /// <param name="navigationProperty">The navigation property this node represents.</param>
        /// <param name="parsedSegments">The path segments parsed in path and query option.</param>
        internal CollectionNavigationNode(SingleResourceNode source, IEdmNavigationProperty navigationProperty,
            List<ODataPathSegment> parsedSegments)
            : this(navigationProperty)
        {
            ExceptionUtils.CheckArgumentNotNull(source, "source");
            this.source = source;
            this.parsedSegments = parsedSegments;

            this.navigationSource = source.NavigationSource != null ? source.NavigationSource.FindNavigationTarget(navigationProperty, BindingPathHelper.MatchBindingPath, this.parsedSegments) : null;
        }

        /// <summary>
        /// Creates a CollectionNavigationNode.
        /// </summary>
        /// <param name="navigationProperty">The navigation property that defines the collection node.</param>
        /// <returns>The collection node.</returns>
        /// <exception cref="System.ArgumentNullException">Throws if the input navigation property is null.</exception>
        /// <exception cref="ArgumentException">Throws if the input navigation doesn't target a collection.</exception>
        private CollectionNavigationNode(IEdmNavigationProperty navigationProperty)
        {
            ExceptionUtils.CheckArgumentNotNull(navigationProperty, "navigationProperty");

            if (navigationProperty.TargetMultiplicity() != EdmMultiplicity.Many)
            {
                throw new ArgumentException(ODataErrorStrings.Nodes_CollectionNavigationNode_MustHaveManyMultiplicity);
            }

            this.navigationProperty = navigationProperty;
            this.collectionTypeReference = navigationProperty.Type.AsCollection();
            this.edmEntityTypeReference = this.collectionTypeReference.ElementType().AsEntityOrNull();
        }

        /// <summary>
        /// Gets the parent node of this Collection Navigation Node.
        /// </summary>
        public SingleResourceNode Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the target multiplicity.
        /// </summary>
        public EdmMultiplicity TargetMultiplicity
        {
            get { return this.navigationProperty.TargetMultiplicity(); }
        }

        /// <summary>
        /// Gets the Navigation Property that defines this collection Node.
        /// </summary>
        /// <value> The navigation property that defines this collection node. </value>
        public IEdmNavigationProperty NavigationProperty
        {
            get { return this.navigationProperty; }
        }

        /// <summary>
        /// Gets a reference to the resource type a single entity in the collection.
        /// </summary>
        public override IEdmTypeReference ItemType
        {
            get { return this.edmEntityTypeReference; }
        }

        /// <summary>
        /// The type of the collection represented by this node.
        /// </summary>
        public override IEdmCollectionTypeReference CollectionType
        {
            get { return this.collectionTypeReference; }
        }

        /// <summary>
        /// Gets the resouce type of a single entity from the collection.
        /// </summary>
        public override IEdmEntityTypeReference EntityItemType
        {
            get { return this.edmEntityTypeReference; }
        }

        /// <summary>
        /// Gets the navigation source containing this collection.
        /// </summary>
        public override IEdmNavigationSource NavigationSource
        {
            get { return this.navigationSource; }
        }

        /// <summary>
        /// Gets the kind of this node.
        /// </summary>
        internal override InternalQueryNodeKind InternalKind
        {
            get
            {
                return InternalQueryNodeKind.CollectionNavigationNode;
            }
        }

        /// <summary>
        /// Accept a <see cref="QueryNodeVisitor{T}"/> that walks a tree of <see cref="QueryNode"/>s.
        /// </summary>
        /// <typeparam name="T">Type that the visitor will return after visiting this token.</typeparam>
        /// <param name="visitor">An implementation of the visitor interface.</param>
        /// <returns>An object whose type is determined by the type parameter of the visitor.</returns>
        /// <exception cref="System.ArgumentNullException">Throws if the input visitor is null.</exception>
        public override T Accept<T>(QueryNodeVisitor<T> visitor)
        {
            ExceptionUtils.CheckArgumentNotNull(visitor, "visitor");
            return visitor.Visit(this);
        }
    }
}