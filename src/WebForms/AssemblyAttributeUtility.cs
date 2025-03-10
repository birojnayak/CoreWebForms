// MIT License.

#if !NET8_0_OR_GREATER
using System.Collections.Generic;
using System.Linq;
#endif
using System.Reflection;
using System.Reflection.Metadata;
using System;

namespace WebForms;

internal static class AssemblyAttributeUtility
{
    public static bool HasAttribute<T>(this MetadataReader reader)
        => reader.GetAttributes<T>().Any();

    public static IEnumerable<CustomAttribute> GetAttributes<T>(this MetadataReader reader)
        => reader.GetAttributes(typeof(T).Name, typeof(T).Namespace);

    public static IEnumerable<CustomAttribute> GetAttributes(this MetadataReader reader, string typeName, string typeNamespace)
    {
        foreach (var a in reader.CustomAttributes)
        {
            var attribute = reader.GetCustomAttribute(a);
            var attributeCtor = attribute.Constructor;

            StringHandle attributeTypeName = default;
            StringHandle attributeTypeNamespace = default;

            if (attributeCtor.Kind == HandleKind.MemberReference)
            {
                var attributeMemberParent = reader.GetMemberReference((MemberReferenceHandle)attributeCtor).Parent;
                if (attributeMemberParent.Kind == HandleKind.TypeReference)
                {
                    var attributeTypeRef = reader.GetTypeReference((TypeReferenceHandle)attributeMemberParent);
                    attributeTypeName = attributeTypeRef.Name;
                    attributeTypeNamespace = attributeTypeRef.Namespace;
                }
            }
            else if (attributeCtor.Kind == HandleKind.MethodDefinition)
            {
                var attributeTypeDefHandle = reader.GetMethodDefinition((MethodDefinitionHandle)attributeCtor).GetDeclaringType();
                var attributeTypeDef = reader.GetTypeDefinition(attributeTypeDefHandle);
                attributeTypeName = attributeTypeDef.Name;
                attributeTypeNamespace = attributeTypeDef.Namespace;
            }

            if (!attributeTypeName.IsNil &&
                !attributeTypeNamespace.IsNil &&
                reader.StringComparer.Equals(attributeTypeName, typeName) &&
                reader.StringComparer.Equals(attributeTypeNamespace, typeNamespace))
            {
                var typeProvider = new AttributeTypeProvider();
                var val = attribute.DecodeValue(typeProvider);
                byte[] data = reader.GetBlobBytes(attribute.Value);
                var t = System.Convert.ToBase64String(data);
                var str = System.Text.Encoding.UTF8.GetString(data);
                   var str1=       System.Text.Encoding.ASCII.GetString(data);
                var str2 = System.Text.Encoding.Unicode.GetString(data);
                var str3 = System.Text.Encoding.UTF32.GetString(data);
                yield return attribute;
            }
        }
    }

}

internal sealed class AttributeTypeProvider : ICustomAttributeTypeProvider<string>
{
    private static readonly Dictionary<PrimitiveTypeCode, Type> PrimitiveTypeMappings =
        new Dictionary<PrimitiveTypeCode, Type>
        {
                    { PrimitiveTypeCode.Void, typeof(void) },
                    { PrimitiveTypeCode.Object, typeof(object) },
                    { PrimitiveTypeCode.Boolean, typeof(bool) },
                    { PrimitiveTypeCode.Char, typeof(char) },
                    { PrimitiveTypeCode.String, typeof(string) },
                    { PrimitiveTypeCode.TypedReference, typeof(TypedReference) },
                    { PrimitiveTypeCode.IntPtr, typeof(IntPtr) },
                    { PrimitiveTypeCode.UIntPtr, typeof(UIntPtr) },
                    { PrimitiveTypeCode.Single, typeof(float) },
                    { PrimitiveTypeCode.Double, typeof(double) },
                    { PrimitiveTypeCode.Byte, typeof(byte) },
                    { PrimitiveTypeCode.SByte, typeof(sbyte) },
                    { PrimitiveTypeCode.Int16, typeof(short) },
                    { PrimitiveTypeCode.UInt16, typeof(ushort) },
                    { PrimitiveTypeCode.Int32, typeof(int) },
                    { PrimitiveTypeCode.UInt32, typeof(uint) },
                    { PrimitiveTypeCode.Int64, typeof(long) },
                    { PrimitiveTypeCode.UInt64, typeof(ulong) }
        };

    public string GetPrimitiveType(PrimitiveTypeCode typeCode)
    {
        if (PrimitiveTypeMappings.TryGetValue(typeCode, out var type))
        {
            return type.FullName!;
        }

        throw new ArgumentOutOfRangeException(nameof(typeCode), typeCode, @"Unexpected type code.");
    }

    public string GetSystemType()
    {
        throw new NotImplementedException();
    }

    public string GetSZArrayType(string elementType)
    {
        throw new NotImplementedException();
    }

    public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
    {
        throw new NotImplementedException();
    }

    public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
    {
        throw new NotImplementedException();
    }

    public string GetTypeFromSerializedName(string name)
    {
        throw new NotImplementedException();
    }

    public PrimitiveTypeCode GetUnderlyingEnumType(string type)
    {
        throw new NotImplementedException();
    }

    public bool IsSystemType(string type)
    {
        throw new NotImplementedException();
    }

    /*
    public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
    {
        var definition = reader.GetTypeDefinition(handle);

        var name = definition.Namespace.IsNil
            ? reader.GetString(definition.Name)
            : reader.GetString(definition.Namespace) + "." + reader.GetString(definition.Name);

        if (IsNested(definition.Attributes))
        {
            var declaringTypeHandle = definition.GetDeclaringType();
            return GetTypeFromDefinition(reader, declaringTypeHandle, 0) + "+" + name;
        }

        return name;
    }


    private static bool IsNested(TypeAttributes flags)
    {
        const TypeAttributes nestedMask = TypeAttributes.NestedFamily | TypeAttributes.NestedPublic;

        return (flags & nestedMask) != 0;
    }

    public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
    {
        var reference = reader.GetTypeReference(handle);

        var name = reference.Namespace.IsNil
            ? reader.GetString(reference.Name)
            : reader.GetString(reference.Namespace) + "." + reader.GetString(reference.Name);

        Handle scope = reference.ResolutionScope;
        return scope.Kind switch
        {
            HandleKind.TypeReference => GetTypeFromReference(reader, (TypeReferenceHandle)scope, 0) + "+" + name,

            // If type refers other module or assembly, don't append them to result.
            // Usually we don't have those assemblies, so we'll be unable to resolve the exact type.
            _ => name,
        };
    }

    public string GetSZArrayType(string elementType)
    {
        return elementType + "[]";
    }

    public string GetSystemType()
    {
        return typeof(Type).FullName!;
    }

    public bool IsSystemType(string type)
    {
        return Type.GetType(type, false) == typeof(Type);
    }

    public string GetTypeFromSerializedName(string name)
    {
        return name;
    }

    public PrimitiveTypeCode GetUnderlyingEnumType(string type)
    {
        var runtimeType = Type.GetType(type, false);

        if (runtimeType != null)
        {
            var underlyingType = runtimeType.GetEnumUnderlyingType();

            foreach (var primitiveTypeMapping in PrimitiveTypeMappings)
            {
                if (primitiveTypeMapping.Value == underlyingType)
                {
                    return primitiveTypeMapping.Key;
                }
            }
        }

        throw new Exception($"Type '{type}' is of unknown TypeCode");
    }*/
}


