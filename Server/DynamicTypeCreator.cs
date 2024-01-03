/*
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;
using Inscryption_Server.DataTypes;
using System.Security.AccessControl;
using System.Xml.Linq;
using System.Web.UI;

namespace Inscryption_Server
{
	internal class DynamicTypeCreator
	{
		const string NAME = "InscriptionDynamicodule";
		static AssemblyBuilder aBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(NAME), AssemblyBuilderAccess.Run);
		static ModuleBuilder mBuilder = aBuilder.DefineDynamicModule(NAME);
		public static Type CreateDynamicType(string name, Dictionary<string, Type> properties)
		{
			TypeBuilder typeBuilder = mBuilder.DefineType(name, TypeAttributes.Public);
			properties.ToList().ForEach(p => AddProperty(typeBuilder, p.Key, p.Value));
			return typeBuilder.CreateType();
		}
		public static void AddProperty(TypeBuilder typeBuilder, string name, Type propertyType)
		{
			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(name, 0, propertyType, null);

			FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + name, propertyType, FieldAttributes.Private);
			MethodBuilder getterBuilder = typeBuilder.DefineMethod("get_" + name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
			MethodBuilder setterBuilder = typeBuilder.DefineMethod("set_" + name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { propertyType });

			ILGenerator getterIL = getterBuilder.GetILGenerator();
			getterIL.Emit(OpCodes.Ldarg_0);
			getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
			getterIL.Emit(OpCodes.Ret);

			ILGenerator setterIL = setterBuilder.GetILGenerator();
			setterIL.Emit(OpCodes.Ldarg_0);
			setterIL.Emit(OpCodes.Ldarg_1);
			setterIL.Emit(OpCodes.Stfld, fieldBuilder);
			setterIL.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(getterBuilder);
			propertyBuilder.SetSetMethod(setterBuilder);
		}
		public static void AddProperty(TypeBuilder typeBuilder, PropertyInfo pi)
		{
			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(pi.Name, 0, pi.PropertyType, null);

			FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + pi.Name, pi.PropertyType, FieldAttributes.Private);
			MethodBuilder getterBuilder = typeBuilder.DefineMethod("get_" + pi.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, pi.PropertyType, Type.EmptyTypes);
			MethodBuilder setterBuilder = typeBuilder.DefineMethod("set_" + pi.Name, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, null, new Type[] { pi.PropertyType });

			ILGenerator getterIL = getterBuilder.GetILGenerator();
			getterIL.Emit(OpCodes.Ldarg_0);
			getterIL.Emit(OpCodes.Call, pi.GetGetMethod());
			getterIL.Emit(OpCodes.Ret);

			ILGenerator setterIL = setterBuilder.GetILGenerator();
			setterIL.Emit(OpCodes.Ldarg_0);
			setterIL.Emit(OpCodes.Ldarg_1);
			setterIL.Emit(OpCodes.Call, pi.GetSetMethod());
			setterIL.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(getterBuilder);
			propertyBuilder.SetSetMethod(setterBuilder);

		}
		internal static Type ExpandType(string name, Type baseType, PropertyInfo[] properties)
		{
			TypeBuilder typeBuilder = mBuilder.DefineType(name, TypeAttributes.Public | TypeAttributes.Class, baseType);
			ConstructorInfo baseConstructor =  baseType.GetConstructors()[0];
			Type[] types =  baseConstructor.GetParameters().Select(i => i.ParameterType).ToArray();
			ConstructorBuilder ctor0 = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, types);

			ILGenerator ctor0IL = ctor0.GetILGenerator();
			ctor0IL.Emit(OpCodes.Ldarg_0);
			int paramCount = baseType.GetConstructors()[0].GetParameters().Count();
			for (int i = 0; i < paramCount; i++)
			{
				ctor0IL.Emit(OpCodes.Ldarga,i);
			}
			ctor0IL.Emit(OpCodes.Call, baseConstructor);
			ctor0IL.Emit(OpCodes.Ret);

			properties.ToList().ForEach(p => AddProperty(typeBuilder, p));
			return typeBuilder.CreateType();
		}
	}
}
*/