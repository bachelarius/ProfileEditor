namespace ProfileEditor.Models {
    public record ProfileImage (Guid PersonId, byte[] ImageData, string ContentType );
}
