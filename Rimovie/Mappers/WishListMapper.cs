namespace Rimovie.Mappers
{
    using Rimovie.Entities;
    using Rimovie.Models.Response;

    public static class WishListMapper
    {
        public static WishListResponseDto ToResponseDto(WishList wishList)
        {
            return new WishListResponseDto
            {
                WishListId = wishList.WishListId,
                Name = wishList.Name
            };
        }

        public static WishList ToEntity(string name, int userId)
        {
            return new WishList
            {
                Name = name,
                UserId = userId
            };
        }
    }
}