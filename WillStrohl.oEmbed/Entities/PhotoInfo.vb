﻿'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2009-2011, Will Strohl
'All rights reserved.
'
'Redistribution and use in source and binary forms, with or without modification, are 
'permitted provided that the following conditions are met:
'
'Redistributions of source code must retain the above copyright notice, this list of 
'conditions and the following disclaimer.
'
'Redistributions in binary form must reproduce the above copyright notice, this list 
'of conditions and the following disclaimer in the documentation and/or other 
'materials provided with the distribution.
'
'Neither the name of Will Strohl nor the names of its contributors may be 
'used to endorse or promote products derived from this software without specific prior 
'written permission.
'
'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
'EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
'OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
'SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
'INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
'TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
'BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
'CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
'ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
'DAMAGE.
'

Imports System.Runtime.Serialization
Imports WillStrohl.API.oEmbed.Constants

Namespace WillStrohl.API.oEmbed

    ''' <summary>
    ''' PhotoInfo - This is the oEmbed type used for representing static photos.
    ''' </summary>
    ''' <remarks></remarks>
    <DataContract()> _
    Public Class PhotoInfo
        Inherits oEmbedInfo
        Implements IPhotoType

#Region " Private Members "

        Private p_Height As Integer = NULL_INTEGER
        Private p_Url As String = NULL_STRING
        Private p_Width As Integer = NULL_INTEGER

#End Region

        Public Sub New()
            ' do nothing
        End Sub

#Region " Properties "

        ''' <summary>
        ''' The source URL of the image. Consumers should be able to insert this URL into an &lt;img&gt; element. Only HTTP and HTTPS URLs are valid.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Required to be returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="height")> _
        Public Property Height() As Integer Implements IPhotoType.Height
            Get
                Return Me.p_Height
            End Get
            Set(ByVal value As Integer)
                Me.p_Height = value
            End Set
        End Property

        ''' <summary>
        ''' The width in pixels of the image specified in the url parameter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Required to be returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="url")> _
        Public Property Url() As String Implements IPhotoType.Url
            Get
                Return Me.p_Url
            End Get
            Set(ByVal value As String)
                Me.p_Url = value
            End Set
        End Property

        ''' <summary>
        ''' The height in pixels of the image specified in the url parameter.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Required to be returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="width")> _
        Public Property Width() As Integer Implements IPhotoType.Width
            Get
                Return Me.p_Width
            End Get
            Set(ByVal value As Integer)
                Me.p_Width = value
            End Set
        End Property

#End Region

    End Class

End Namespace