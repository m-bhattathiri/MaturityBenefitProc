/* MaturityBenefitProc - Client-side helpers for insurance maturity benefit forms */
(function ($) {
    'use strict';

    var MaturityApp = window.MaturityApp || {};

    // Format amount as Indian currency (INR)
    MaturityApp.formatCurrency = function (amount) {
        if (isNaN(amount) || amount === null || amount === undefined) {
            return '\u20B90.00';
        }
        var num = parseFloat(amount);
        var isNegative = num < 0;
        num = Math.abs(num);
        var parts = num.toFixed(2).split('.');
        var intPart = parts[0];
        var decPart = parts[1];
        var lastThree = intPart.substring(intPart.length - 3);
        var remaining = intPart.substring(0, intPart.length - 3);
        if (remaining !== '') {
            lastThree = ',' + lastThree;
        }
        var formatted = remaining.replace(/\B(?=(\d{2})+(?!\d))/g, ',') + lastThree;
        return (isNegative ? '-' : '') + '\u20B9' + formatted + '.' + decPart;
    };

    // Format date as DD-MMM-YYYY (e.g., 15-Mar-2017)
    MaturityApp.formatDate = function (dateStr) {
        if (!dateStr) return '';
        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                       'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var dt = new Date(dateStr);
        if (isNaN(dt.getTime())) return dateStr;
        var day = dt.getDate();
        var month = months[dt.getMonth()];
        var year = dt.getFullYear();
        return (day < 10 ? '0' : '') + day + '-' + month + '-' + year;
    };

    // Validate policy number format (2-4 uppercase letters followed by 6-10 digits)
    MaturityApp.validatePolicyNumber = function (policyNo) {
        if (!policyNo || typeof policyNo !== 'string') return false;
        return /^[A-Z]{2,4}\d{6,10}$/.test(policyNo.trim());
    };

    // Validate PAN number (Indian format: ABCDE1234F)
    MaturityApp.validatePAN = function (pan) {
        if (!pan || typeof pan !== 'string') return false;
        return /^[A-Z]{5}\d{4}[A-Z]$/.test(pan.trim().toUpperCase());
    };

    // Validate IFSC code (e.g., SBIN0001234)
    MaturityApp.validateIFSC = function (ifsc) {
        if (!ifsc || typeof ifsc !== 'string') return false;
        return /^[A-Z]{4}0[A-Z0-9]{6}$/.test(ifsc.trim().toUpperCase());
    };

    // Calculate maturity benefit after tax deduction
    MaturityApp.calculateNetBenefit = function (grossAmount, taxPercent) {
        var gross = parseFloat(grossAmount) || 0;
        var tax = parseFloat(taxPercent) || 0;
        if (gross <= 0) return 0;
        var taxAmount = gross * (tax / 100);
        return Math.round((gross - taxAmount) * 100) / 100;
    };

    // Attach currency formatting to amount fields on blur
    MaturityApp.initAmountFields = function () {
        $('.amount-input').on('blur', function () {
            var val = $(this).val().replace(/[^\d.]/g, '');
            if (val && !isNaN(parseFloat(val))) {
                $(this).val(parseFloat(val).toFixed(2));
            }
        });
    };

    // Form validation for maturity claim submission
    MaturityApp.validateClaimForm = function (formSelector) {
        var $form = $(formSelector);
        var isValid = true;
        $form.find('.input-error').removeClass('input-error');
        $form.find('.validation-msg').remove();

        var policyNo = $form.find('[name="PolicyNumber"]').val();
        if (!MaturityApp.validatePolicyNumber(policyNo)) {
            MaturityApp.showFieldError($form.find('[name="PolicyNumber"]'),
                'Enter a valid policy number (e.g., LIC1234567)');
            isValid = false;
        }

        var pan = $form.find('[name="PanNumber"]').val();
        if (pan && !MaturityApp.validatePAN(pan)) {
            MaturityApp.showFieldError($form.find('[name="PanNumber"]'),
                'Enter a valid PAN (e.g., ABCDE1234F)');
            isValid = false;
        }

        var ifsc = $form.find('[name="IfscCode"]').val();
        if (ifsc && !MaturityApp.validateIFSC(ifsc)) {
            MaturityApp.showFieldError($form.find('[name="IfscCode"]'),
                'Enter a valid IFSC code (e.g., SBIN0001234)');
            isValid = false;
        }

        return isValid;
    };

    // Show inline validation error on a field
    MaturityApp.showFieldError = function ($field, message) {
        $field.addClass('input-error');
        $field.after('<span class="validation-msg text-danger">' + message + '</span>');
    };

    // Initialize on document ready
    $(document).ready(function () {
        MaturityApp.initAmountFields();
        $('.currency-display').each(function () {
            var raw = $(this).text().trim();
            if (raw && !isNaN(parseFloat(raw))) {
                $(this).text(MaturityApp.formatCurrency(raw));
            }
        });
        $('.date-display').each(function () {
            var raw = $(this).text().trim();
            $(this).text(MaturityApp.formatDate(raw));
        });
    });

    window.MaturityApp = MaturityApp;

})(jQuery);
