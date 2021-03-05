mergeInto(LibraryManager.library, 
{
  BindWebGLTexture: function (texture) 
  {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
  },
});