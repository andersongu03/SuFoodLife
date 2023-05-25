const vm = new Vue({
    el: '.couponManagement',
    data: {
        pageName: '優惠券管理',
        keyword: '',
        toast:'',
        editId: undefined,
        delId: undefined,
        popupShowing: {
            showPopup: false,
            /*cardPopupBody4Del: false,*/
        },
        toastHintStyle: {
            fadeInUp: false
        },
        CreateOrEditOrDelete: '',
        coupons: [],
        editCouponList: {
            CouponId: this.editId,
            CouponName: '666',
            CouponDescription: '666',
            CouponMinusCost: '777',
            MinimumPurchasingAmount: '777',
            CouponStartDate: '',
            CouponEndDate: ''
        },
        manage: {
            CouponId: 0,
            CouponName: '',
            CouponDescription: '',
            CouponMinusCost: '',
            MinimumPurchasingAmount: '',
            CouponStartDate: '',
            CouponEndDate: ''
        },
    },
    methods: {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
                this.toastHintStyle.fadeInUp = false
            }, 2100)
        },
        createCoupon() {
            this.CreateOrEditOrDelete = "Create";
            this.popupShowing.showPopup = true;
        },
        closeCoupon() {
            this.popupShowing.showPopup = false;
        },
        closePopupShowHint() {
            this.popupShowing.showPopup = false;
            this.toastHint();
        },
        editCoupon(item) {
            this.CreateOrEditOrDelete = "Edit";
            this.popupShowing.showPopup = true;
            this.editCouponList = item;
            this.editId = item.couponId;
        },
        delCoupon(c) {
            this.CreateOrEditOrDelete = "Delete";
            this.popupShowing.showPopup = true;
            this.delId = c.couponId;
            console.log(this.delId);
        },
        GetCouponInfo() {
            let _this = this;
            couponInfo = axios('/BackStage/CouponManagement/GetCoupons').then(response => {
                _this.coupons = response.data
            });
        },
        CreateCoupon() {
            let _this = this;
            axios.post('/BackStage/CouponManagement/Create', this.manage, {
                headers: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                this.toast = response.data
                this.closePopupShowHint()
                _this.GetCouponInfo()
            })
        },
        EditCoupon() {
            let _this = this;
            var tempData = {
                CouponId: _this.editId,
                CouponName: _this.editCouponList.couponName,
                CouponDescription: _this.editCouponList.couponDescription,
                CouponMinusCost: _this.editCouponList.couponMinusCost,
                MinimumPurchasingAmount: _this.editCouponList.minimumPurchasingAmount,
                CouponStartDate: _this.editCouponList.couponstartdate2String,
                CouponEndDate: _this.editCouponList.couponenddate2String,
            };
            axios.put("/BackStage/CouponManagement/Edit/", tempData, {
                headers: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                this.toast = response.data
                /*alert(response.data)*/
                this.closePopupShowHint()
                _this.GetCouponInfo()
            })
        },
        DelCoupon(id) {
            let _this = this;
            //console.log(id, 'id')
            //console.log(_this.id, 'id'); //undefined
            axios.delete(`/BackStage/CouponManagement/Delete/${id}`)
                .then(response => {
                    this.toast = response.data
                    this.closePopupShowHint()
                    _this.GetCouponInfo();
                }).catch(error => console.log(error));
        }
    },
    computed: {
        filterCoupons() {
            let keywordLowerCase = this.keyword.toLowerCase();
            return this.coupons.filter(c => {
                return c.couponName.toLowerCase().indexOf(keywordLowerCase) !== -1;
            })
        }
    },
    mounted() {
        this.GetCouponInfo();
    }
});