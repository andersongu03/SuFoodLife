const vm = new Vue({
    el: '.couponDetails',
    data: {
        pageName: '我的優惠券',
        couponList: [
            //{ name: '蔬Food', description: '生鮮蔬果現折$100', endDate: '2023-05-31' },
            //{ name: '真D蔬Food', description: '滿2000折500', endDate: '2023-12-31' },
            //{ name: 'Food可敵國', description: '全場88折', endDate: '2023-12-31' },
            //{ name: 'DDDD', description: '全場85折', endDate: '2023-05-23' },
            //{ name: 'FFFF', description: '不限金額9折', endDate: '2023-01-01' },
            //{ name: 'Oct', description: '不限金額9折', endDate: '2023-10-01' },
        ],
    },
    computed: {
        isValid() {
            const today = new Date();
            return this.couponList.some(c => new Date(c.couponenddate2String >= today));
        },
        validCoupons() {
            const today = new Date();
            return this.couponList.filter(c => new Date(c.couponenddate2String) >= today);
        },
        invalidCoupons() {
            const today = new Date();
            return this.couponList.filter(c => new Date(c.couponenddate2String) < today);
        },
    },
    methods: {
        insertExistCoupon() {
            console.log('123');
        },
        GetCouponFromDb() {
            let _this = this;
            couponFromDb = axios('/Coupon/GetCouponsForFront').then(response => {
                _this.couponList = response.data;
            });
        },
    },
    mounted() {
        this.GetCouponFromDb();
    }
});