document.addEventListener('DOMContentLoaded', function() {
    // MFA Code Auto-submit
    const mfaCodeInput = document.querySelector('input[name="code"]');
    if (mfaCodeInput) {
        mfaCodeInput.addEventListener('input', function(e) {
            if (this.value.length === 6) {
                this.form.submit();
            }
        });
    }

    // Recovery Code Toggle
    const recoveryLink = document.getElementById('useRecoveryLink');
    if (recoveryLink) {
        recoveryLink.addEventListener('click', function(e) {
            e.preventDefault();
            document.getElementById('mfaSection').innerHTML = `
                <h4>Enter Recovery Code</h4>
                <form action="/Mfa/UseRecovery" method="post">
                    <input type="text" name="code" placeholder="Recovery code" />
                    <button type="submit" class="btn btn-secondary">Verify</button>
                </form>
            `;
        });
    }
});