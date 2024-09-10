import { useEffect, useState } from 'react';
import { PaymentForm, CreditCard } from 'react-square-web-payments-sdk';
import Success from './SuccessBanner';

function Card() {
    const [showSuccess, setShowSuccess] = useState(false);
    const [paymentRes, setPaymentRes] = useState(null);
    useEffect(() => {
        if (paymentRes) {
            const status = paymentRes.status;
            console.log(`Status: ${status}`)
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
        }
    }, [paymentRes, showSuccess]);

    useEffect(() => {
        if (showSuccess) {
            console.log(`showSuccess: ${showSuccess}`);
        }
        
    }, [showSuccess])

    return (
        <div>
            <PaymentForm
                applicationId="sandbox-sq0idb-M8bxvTOpeTAZ3vZVDxpomg"
                locationId="LMM26ACKFGN8W"
                cardTokenizeResponseReceived={async (token, verifiedBuyer) => {
                    //TODO: add error handlers for card (e.g. invalid CVV, invalid Card, invalid Postal, etc.)
                    console.log(token.details.billing?.postalCode ?? null);
                    const body = {
                        token: token.token,
                        status: token.status,
                        method: token.details.method,
                        cardBrand: token.details.card.brand,
                        cardLast4: token.details.card.last4,
                        cardExpMonth: token.details.card.expMonth,
                        cardExpYear: token.details.card.expYear
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
                    console.log(token);
                    let res = (await response.json());
                    setPaymentRes(res);
                }}>
                <CreditCard></CreditCard>
            </PaymentForm>
            {showSuccess && (<Success/>) }

        </div>
    );
}
export default Card;