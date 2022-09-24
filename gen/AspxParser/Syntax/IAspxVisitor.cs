// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.SystemWebAdapters.Transpiler.Syntax;

public interface IAspxVisitor<out T>
{
    T Visit(AspxNode.Root node);
    T Visit(AspxNode.OpenHtmlTag node);
    T Visit(AspxNode.SelfClosingHtmlTag node);
    T Visit(AspxNode.OpenAspxTag node);
    T Visit(AspxNode.SelfClosingAspxTag node);
    T Visit(AspxNode.CloseAspxTag node);
    T Visit(AspxNode.CloseHtmlTag node);
    T Visit(AspxNode.AspxDirective node);
    T Visit(AspxNode.DataBinding node);
    T Visit(AspxNode.CodeRender node);
    T Visit(AspxNode.CodeRenderExpression node);
    T Visit(AspxNode.CodeRenderEncode node);
    T Visit(AspxNode.Literal node);
}
