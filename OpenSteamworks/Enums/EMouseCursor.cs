using System;

namespace OpenSteamworks.Enums;

public enum EMouseCursor : UInt32
{
    dc_user = 0,
    dc_none,
    dc_arrow,
    dc_ibeam,
    dc_hourglass,
    dc_waitarrow,
    dc_crosshair,
    dc_up,
    dc_sizenw,
    dc_sizese,
    dc_sizene,
    dc_sizesw,
    dc_sizew,
    dc_sizee,
    dc_sizen,
    dc_sizes,
    dc_sizewe,
    dc_sizens,
    dc_sizeall,
    dc_no,
    dc_hand,
    /// <summary>
    /// don't show any custom cursor, just use your default
    /// </summary>
    dc_blank,
    dc_middle_pan,
    dc_north_pan,
    dc_north_east_pan,
    dc_east_pan,
    dc_south_east_pan,
    dc_south_pan,
    dc_south_west_pan,
    dc_west_pan,
    dc_north_west_pan,
    dc_alias,
    dc_cell,
    dc_colresize,
    dc_copycur,
    dc_verticaltext,
    dc_rowresize,
    dc_zoomin,
    dc_zoomout,
    dc_help,
    dc_custom,

    /// <summary>
    /// custom cursors start from this value and up
    /// </summary>
    dc_last,
};