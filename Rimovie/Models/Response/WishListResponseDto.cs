namespace Rimovie.Models.Response
{
    public class WishListResponseDto
    {
        public int WishListId { get; set; }         // wishlistid
        public string Name { get; set; }    // nombre de la lista
        public int UserId { get; set; }     // dueño de la lista
    }

}
