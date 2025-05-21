using FluentMigrator.Expressions;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using System.Collections.Concurrent;
using System.Reflection;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Common;
using System;

namespace TheTecniQ.Data.Mapping
{
    /// <summary>
    /// LINQ To DB metadata reader for schema created by FluentMigrator
    /// </summary>
    public partial class FluentMigratorMetadataReader : IMetadataReader
    {
        #region Ctor

        public FluentMigratorMetadataReader()
        {
        }

        #endregion

        #region Utils

        protected static T GetAttribute<T>(Type type, MemberInfo memberInfo) where T : Attribute
        {
            Attribute attribute = Types.GetOrAdd((type, memberInfo), t =>
            {

                if (typeof(T) != typeof(MappingAttribute))
                {
                    return null;
                }

                bool isIgnoreColumn = memberInfo.GetCustomAttribute(typeof(NoColumnMap)) != null;
                if (memberInfo.Name == "Id")
                {
                    return new ColumnAttribute
                    {
                        //Name = "Id",
                        IsPrimaryKey = true,
                        //IsColumn = true,
                        //CanBeNull = false,
                        IsIdentity = true
                    };
                }
                if (isIgnoreColumn)
                {
                    return new ColumnAttribute
                    {
                        IsColumn = false
                    };
                }

                return null;

            });

            return (T)attribute;
        }

        /*protected static T[] GetAttributes<T>(Type type, Type attributeType, MemberInfo memberInfo = null)
            where T : Attribute
        {
            if (type.IsSubclassOf(typeof(BaseEntity)) && typeof(T) == attributeType && GetAttribute<T>(type, memberInfo) is T attr)
            {
                return new[] { attr };
            }

            return Array.Empty<T>();
        }*/
        protected static T[] GetAttributes<T>(Type type, Type attributeType, MemberInfo memberInfo = null)
            where T : Attribute
        {
            // Check if the type is a subclass of BaseEntity
            if (type.IsSubclassOf(typeof(BaseEntity)))
            {
                // If a memberInfo is provided, call GetAttribute<T> to retrieve attributes from the member
                if (memberInfo != null)
                {
                    // Call GetAttribute<T> to retrieve the attribute applied to the member
                    T attribute = GetAttribute<T>(type, memberInfo);

                    // If the attribute is not null, return it in an array
                    if (attribute != null)
                    {
                        return [attribute];
                    }
                }
                else
                {
                    // If no memberInfo is provided, retrieve attributes applied to the type
                    // Check if the attribute applied to the type matches the specified attributeType
                    if (attributeType != null && typeof(T) == attributeType)
                    {
                        // Retrieve attributes applied to the type
                        T[] attributes = (T[])type.GetCustomAttributes(attributeType, false);

                        // If attributes are found, return them
                        if (attributes.Length > 0)
                        {
                            return attributes;
                        }
                    }
                }
            }

            // If no attributes are found or conditions are not met, return an empty array
            return [];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets attributes of specified type, associated with specified type.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="type">Attributes owner type.</param>
        /// <returns>Attributes of specified type.</returns>
        public virtual MappingAttribute[] GetAttributes(Type type)
        {
            return GetAttributes<MappingAttribute>(type, typeof(TableAttribute));
        }

        /// <summary>
        /// Gets attributes of specified type, associated with specified type member.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="type">Member's owner type.</param>
        /// <param name="memberInfo">Attributes owner member.</param>
        /// <returns>Attributes of specified type.</returns>
        public virtual MappingAttribute[] GetAttributes(Type type, MemberInfo memberInfo)
        {
            return GetAttributes<MappingAttribute>(type, typeof(ColumnAttribute), memberInfo);
        }

        /// <summary>
        /// Gets the dynamic columns defined on given type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>All dynamic columns defined on given type</returns>
        public MemberInfo[] GetDynamicColumns(Type type)
        {
            return [];
        }

        /// <summary>
        /// Should return a unique ID for cache purposes. If the implemented Metadata reader returns instance-specific
        /// data you'll need to calculate a unique value based on content. Otherwise just use a static const
        /// e.g. $".{nameof(YourMetadataReader)}."
        /// </summary>
        /// <returns>The object ID as string</returns>
        public string GetObjectID()
        {
            return $".{nameof(FluentMigratorMetadataReader)}.";
        }

        #endregion

        #region Properties

        protected static ConcurrentDictionary<(Type, MemberInfo), Attribute> Types { get; } = new ConcurrentDictionary<(Type, MemberInfo), Attribute>();
        protected static ConcurrentDictionary<Type, CreateTableExpression> Expressions { get; } = new ConcurrentDictionary<Type, CreateTableExpression>();

        #endregion
    }
}
