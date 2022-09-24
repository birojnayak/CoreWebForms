// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.SystemWebAdapters.Transpiler.Syntax;

internal readonly struct DirectiveDetails
{
    private readonly AspxNode.AspxDirective _directive;

    public DirectiveDetails(AspxNode.AspxDirective directive)
    {
        _directive = directive;
    }

    public bool IsPage => string.Equals(_directive.Name, "Page", StringComparison.OrdinalIgnoreCase);

    public bool IsMasterPage => string.Equals(_directive.Name, "Master", StringComparison.OrdinalIgnoreCase);

    public string MasterPageFile => GetAttribute();

    public string Language => GetAttribute();

    public string Inherits => GetAttribute() ?? DefaultInherits;

    private string DefaultInherits => IsPage ? "global::System.Web.UI.Page" : "global::System.Web.UI.MasterPage";

    public bool AutoEventWireup => bool.TryParse(GetAttribute(), out var result) && result;

    public string Title => GetAttribute();

    public string CodeBehind => GetAttribute();

    private string GetAttribute([CallerMemberName] string name = null!)
        => _directive.Attributes[name];
}
