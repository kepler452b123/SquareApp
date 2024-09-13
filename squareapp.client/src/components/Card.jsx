import { useEffect, useState } from 'react';
import { PaymentForm, CreditCard } from 'react-square-web-payments-sdk';
import Success from './SuccessBanner';
import '../App.css'; // Importing the CSS file for styles

function Card() {
    const [showSuccess, setShowSuccess] = useState(false);
    const [paymentRes, setPaymentRes] = useState(null);
    const [amount, setAmount] = useState(''); // New state for amount
    const [confirmedAmount, setConfirmedAmount] = useState(null); // Confirmed amount

    useEffect(() => {
        if (paymentRes) {
            const status = paymentRes.status;
            console.log(`Status: ${status}`);
            if (status === "COMPLETED") {
                setShowSuccess(true);
                setTimeout(() => {
                    setPaymentRes(null);
                }, 5000);
            }
        }
        if (paymentRes === null && showSuccess === true) {
            console.log("Payment information has been cleared");
            setShowSuccess(false);
            setConfirmedAmount(null);
        }
    }, [paymentRes, showSuccess]);

    useEffect(() => {
        if (showSuccess) {
            console.log(`showSuccess: ${showSuccess}`);
        }
    }, [showSuccess]);

    const handleAmountChange = (e) => {
        setAmount(e.target.value); // Update amount state on input change
    };

    const handleConfirmAmount = () => {
        setConfirmedAmount(amount); // Confirm the amount when arrow button is clicked
        console.log(`Amount Set to: ${amount}`);
    };

    const handleSubmitPayment = async (token) => {
        const body = {
            token: token.token,
            status: token.status,
            method: token.details.method,
            cardBrand: token.details.card.brand,
            cardLast4: token.details.card.last4,
            cardExpMonth: token.details.card.expMonth,
            cardExpYear: token.details.card.expYear,
            amount: confirmedAmount, // Include confirmed amount in the body
        };
        if (token.details.billing?.postalCode) {
            body.billingPostalCode = token.details.billing.postalCode;
        }
        const response = await fetch("api/payments/sendToken", {
            method: "POST",
            headers: {
                "Content-type": "application/json",
            },
            body: JSON.stringify(body),
        });
        let res = await response.json();
        setPaymentRes(res);
    };

    return (
        <div className="payment-container">
            <div className="amount-input-container">
                <label htmlFor="amount" className="input-label">Amount: </label>
                <input
                    type="number"
                    id="amount"
                    value={amount}
                    onChange={handleAmountChange} // Handle amount input
                    placeholder="Enter amount"
                    className="amount-input"
                />
                <button onClick={handleConfirmAmount} className="confirm-button">
                    &#x27A4; {/* Arrow symbol */}
                </button>
            </div>
            {confirmedAmount && <p className="confirmed-amount">Amount set: ${confirmedAmount}</p>}

            <PaymentForm
                applicationId="sandbox-sq0idb-M8bxvTOpeTAZ3vZVDxpomg"
                locationId="LMM26ACKFGN8W"
                cardTokenizeResponseReceived={async (token, verifiedBuyer) => {
                    // Process the payment with the amount and token
                    await handleSubmitPayment(token);
                }}
            >
                <CreditCard></CreditCard>
            </PaymentForm>

            {showSuccess && (<Success />)}
        </div>
    );
}

export default Card;
