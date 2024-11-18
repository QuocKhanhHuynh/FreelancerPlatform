function formatCurrency(input) {
    // Xóa tất cả ký tự không phải số
    const value = input.value.replace(/\D/g, '');

    // Định dạng lại theo kiểu số tiền với dấu phẩy
    const formattedValue = value.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    //const formattedValue = value.toLocaleString();

    // Cập nhật lại giá trị với đơn vị "đ"
    input.value = formattedValue /*+ 'đ'*/;
}
function convertFormatCurrencyToNumber(money) {
    // Loại bỏ tất cả các dấu phân cách hàng nghìn (dấu chấm)
    const cleanedCurrency = money.replace(/\./g, "")/*.replace("đ", "")*/.trim();

    // Chuyển chuỗi đã làm sạch thành số nguyên
    const result = Number(cleanedCurrency);
    return result;
}