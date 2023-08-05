using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql(@"
           DECLARE @UserID as INT
            --------------------------
            --Create User
            --------------------------
            IF not exists (select Id from Users where Username='Demo')
            insert into Users(Username,Password, PasswordKey,UpdatedOn,UpdatedBy)
            select 'Demo',
            0x4D5544D09B8319B423F6D4E054360D5289B57A98781A66B276E00C57919FDCD599BF45623D48CC81F535748F560AF0F70C8C7F3B4C3DB672562B5DD0E5E7C297,
            0x44A0BD5BFD689DF399346200A1117C33BEDF5869C17A7CB3DC6D8598A93845DB333B379AA90931D8D4E5F2CC7B1A4A96A7DB71B186DBCDCDC53B0A95440E4EDD7473668627970FBD9BB0BA17530CCAB2D9446A1902BD6AC12FE691FE09DD78A43398B89111056145843060026A414FFA8C5E75B474E187AD753D2872038D9FDD,
            getdate(),
            0

            SET @UserID = (select id from Users where Username='Demo')

            --------------------------
            --Seed Property Types
            --------------------------
            IF not exists (select PropertyTypeName from PropertyTypes where PropertyTypeName='House')
            insert into PropertyTypes(PropertyTypeName,UpdatedOn,UpdatedBy)
            select 'House', GETDATE(),@UserID

            IF not exists (select PropertyTypeName from PropertyTypes where PropertyTypeName='Apartment')
            insert into PropertyTypes(PropertyTypeName,UpdatedOn,UpdatedBy)
            select 'Apartment', GETDATE(),@UserID

            IF not exists (select PropertyTypeName from PropertyTypes where PropertyTypeName='Duplex')
            insert into PropertyTypes(PropertyTypeName,UpdatedOn,UpdatedBy)
            select 'Duplex', GETDATE(),@UserID


            --------------------------
            --Seed Furnishing Types
            --------------------------
            IF not exists (select FurnishingTypeName from FurnishingTypes where FurnishingTypeName='Fully')
            insert into FurnishingTypes(FurnishingTypeName, UpdatedOn, UpdatedBy)
            select 'Fully', GETDATE(),@UserID

            IF not exists (select FurnishingTypeName from FurnishingTypes where FurnishingTypeName='Semi')
            insert into FurnishingTypes(FurnishingTypeName, UpdatedOn, UpdatedBy)
            select 'Semi', GETDATE(),@UserID

            IF not exists (select FurnishingTypeName from FurnishingTypes where FurnishingTypeName='Unfurnished')
            insert into FurnishingTypes(FurnishingTypeName, UpdatedOn, UpdatedBy)
            select 'Unfurnished', GETDATE(),@UserID

            --------------------------
            --Seed Cities
            --------------------------
            IF not exists (select top 1 id from Cities)
            Insert into Cities(CityName,UpdatedBy,UpdatedOn,Country)
            select 'New York',@UserID,getdate(),'USA'
            union
            select 'Houston',@UserID,getdate(),'USA'
            union
            select 'Los Angeles',@UserID,getdate(),'USA'
            union
            select 'New Delhi',@UserID,getdate(),'India'
            union
            select 'Bangalore',@UserID,getdate(),'India'

            --------------------------
            --Seed Properties
            --------------------------
            --Seed property for sell
            IF not exists (select top 1 PropertyName from Properties where PropertyName='White House Demo')
            insert into Properties(SellRent,PropertyName,PropertyTypeId,BHK,FurnishingTypeId,Price,BuiltArea,CarpetArea,Address,
            Address2,CityId,FloorNo,TotalFloors,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,UpdatedOn,UpdatedBy)
            select 
            1, --Sell Rent
            'White House Demo', --Name
            (select Id from PropertyTypes where PropertyTypeName='Apartment'), --Property Type ID
            2, --BHK
            (select Id from FurnishingTypes where FurnishingTypeName='Fully'), --Furnishing Type ID
            1800, --Price
            1400, --Built Area
            900, --Carpet Area
            '6 Street', --Address
            'Golf Course Road', -- Address2
            (select top 1 Id from Cities), -- City ID
            3, -- Floor No
            3, --Total Floors
            1, --Ready to Move
            'East', --Main Entrance
            0, --Security
            1, --Gated
            300, -- Maintenance
            '2019-01-01', -- Establishment or Posession on
            0, --Age
            'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
            Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', --Description
            GETDATE(), --Posted on
            @UserID, --Posted by
            GETDATE(), --Last Updated on
            @UserID --Last Updated by

            ---------------------------
            --Seed property for rent
            ---------------------------
            IF not exists (select top 1 PropertyName from Properties where PropertyName='Birla House Demo')
            insert into Properties(SellRent,PropertyName,PropertyTypeId,BHK,FurnishingTypeId,Price,BuiltArea,CarpetArea,Address,
            Address2,CityId,FloorNo,TotalFloors,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,UpdatedOn,UpdatedBy)
            select 
            2, --Sell Rent
            'Birla House Demo', --Name
            (select Id from PropertyTypes where PropertyTypeName='Apartment'), --Property Type ID
            2, --BHK
            (select Id from FurnishingTypes where FurnishingTypeName='Fully'), --Furnishing Type ID
            1800, --Price
            1400, --Built Area
            900, --Carpet Area
            '6 Street', --Address
            'Golf Course Road', -- Address2
            (select top 1 Id from Cities), -- City ID
            3, -- Floor No
            3, --Total Floors
            1, --Ready to Move
            'East', --Main Entrance
            0, --Security
            1, --Gated
            300, -- Maintenance
            '2019-01-01', -- Establishment or Posession on
            0, --Age
            'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
            Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', --Description
            GETDATE(), --Posted on
            @UserID, --Posted by
            GETDATE(), --Last Updated on
            @UserID --Last Updated by

           ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql(@"--------------------
            --Seeding Down
            --------------------
            DECLARE @UserID as int
            SET @UserID = (select id from Users where Username='Demo')
            delete from Users where Username='Demo'
            delete from PropertyTypes where UpdatedBy=@UserID
            delete from FurnishingTypes where UpdatedBy=@UserID
            delete from Cities where UpdatedBy=@UserID
            delete from Properties where PostedBy=@UserId
           ");
        }
    }
}
