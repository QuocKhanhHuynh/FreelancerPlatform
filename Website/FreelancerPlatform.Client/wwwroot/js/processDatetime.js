function timeDifference(dbTime) {
    // Lấy thời gian hiện tại
    const currentTime = new Date();

    // Tính độ chênh lệch thời gian (đơn vị: milliseconds)
    const differenceInMs = currentTime - new Date(dbTime);

    // Tính các khoảng thời gian cơ bản
    const differenceInMinutes = Math.floor(differenceInMs / (1000 * 60));
    const differenceInHours = Math.floor(differenceInMs / (1000 * 60 * 60));
    const differenceInDays = Math.floor(differenceInMs / (1000 * 60 * 60 * 24));

    let result;

    if (differenceInMinutes < 60) {
        // Nếu độ chênh lệch nhỏ hơn 1 giờ, hiển thị theo phút
        result = `${differenceInMinutes} phút trước`;
    } else if (differenceInHours < 24) {
        // Nếu độ chênh lệch nhỏ hơn 24 giờ, hiển thị theo giờ
        result = `${differenceInHours} giờ trước`;
    } else {
        // Nếu độ chênh lệch lớn hơn hoặc bằng 24 giờ, hiển thị theo ngày
        result = `${differenceInDays} ngày trước`;
    }

    return result;
}