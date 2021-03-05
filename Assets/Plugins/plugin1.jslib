var myLib=
{
    $dependencies:{},
    CallExternal: function(texture)
    {
        TestExternalJS(texture);
    },
    BindWebGLTexture: function (texture) 
    {
        GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    }
};

autoAddDeps(myLib,'$dependencies');
mergeInto(LibraryManager.library, myLib);