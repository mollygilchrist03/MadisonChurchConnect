/*
 * Molly Gilchrist
 * 2/5/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.DomainModels;
using MadisonChurchConnect.Models.ViewModels;

public static class SermonMapper
{
    /// <summary>
    /// map from domain model to view model
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static SermonViewModel ToViewModel(SermonDomainModel domainSermon)
    {
        // declare and init
        SermonViewModel viewSermon = new SermonViewModel
        {
            // map the properties of the sermon
            SermonId = domainSermon.SermonId,
            SermonTitle = domainSermon.SermonTitle,
            Speaker = domainSermon.Speaker,
            SermonDate = domainSermon.SermonDate,
            VideoUrl = domainSermon.VideoUrl,
            Summary = domainSermon.Summary,
            Series = domainSermon.Series,
            IsFeatured = domainSermon.IsFeatured
        };

        // return the mapped view sermon
        return viewSermon;
    }

    /// <summary>
    /// map from view model to domain model
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    public static SermonDomainModel ToDomainModel(SermonViewModel viewSermon)
    {
        // declare and init
        SermonDomainModel domainSermon;

        // check for null view sermon
        if (viewSermon == null)
        {
            // if null, return an empty domain sermon
            return new SermonDomainModel();
        }

        // create a domain sermon based on view sermon
        domainSermon = new SermonDomainModel
        {
            SermonId = viewSermon.SermonId,
            SermonTitle = viewSermon.SermonTitle,
            Speaker = viewSermon.Speaker,
            SermonDate = viewSermon.SermonDate,
            VideoUrl = viewSermon.VideoUrl,
            Summary = viewSermon.Summary,
            Series = viewSermon.Series,
            IsFeatured = viewSermon.IsFeatured
        };

        // return domain sermon
        return domainSermon;
    }
}
